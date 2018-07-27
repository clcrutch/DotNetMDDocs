using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using Mono.Cecil;

using TypeAttributes = System.Reflection.TypeAttributes;

namespace DotNetDocs
{
    public class TypeDocumentation : DocumentationBase
    {
        protected TypeDefinition TypeDefinition => (TypeDefinition)MemberDefinition;

        public MethodDocumentation[] ConstructorDocumentations { get; private set; }

        public AssemblyDocumentation DeclaringAssembly { get; private set; }

        public FieldDocumentation[] FieldDocumentations { get; private set; }

        public MethodDocumentation[] MethodDocumentations { get; private set; }

        public string Namespace => TypeDefinition.Namespace;

        public PropertyDocumentation[] PropertyDocumentations { get; private set; }

        public System.Reflection.Metadata.TypeDefinition ReflectionTypeDefinition { get; private set; }

        public TypeAttributes TypeAttributes => (TypeAttributes)TypeDefinition.Attributes;

        protected internal TypeDocumentation(TypeDefinition typeDefinition, XElement xElement, AssemblyDocumentation declaringAssembly)
            : base(typeDefinition, xElement)
        {
            DeclaringAssembly = declaringAssembly;

            var typeDefs = from t in declaringAssembly.PEFile.Metadata.TypeDefinitions
                           select declaringAssembly.PEFile.Metadata.GetTypeDefinition(t);

            this.ReflectionTypeDefinition = (from t in typeDefs
                                             where declaringAssembly.PEFile.Metadata.GetString(t.Namespace) == Namespace &&
                                                declaringAssembly.PEFile.Metadata.GetString(t.Name) == Name
                                             select t).Single();

            ConstructorDocumentations = GetConstructorDocumentations(typeDefinition, xElement.Document);
            FieldDocumentations = GetFieldDocumentations(typeDefinition, xElement.Document);
            PropertyDocumentations = GetPropertyDocumentations(typeDefinition, xElement.Document);
            MethodDocumentations = GetMethodDocumentations(typeDefinition, xElement.Document);

            var name = new FullTypeName(FullName);
            this.Declaration = this.DeclaringAssembly.Decompiler.DecompileTypeAsString(name);
            this.Declaration = this.Declaration.Substring(this.Declaration.IndexOf('{') + 1);
            this.Declaration = this.Declaration.Substring(0, this.Declaration.IndexOf('{')).Trim();
        }

        private bool IsSameOverload(MethodDefinition methodDefinition, XElement xElement) => 
            methodDefinition.HasParameters == xElement.Descendants().Any(x => x.Name == "param") &&
            methodDefinition.Parameters.Count == xElement.Descendants().Count(x => x.Name == "param") &&
            methodDefinition.Parameters.Select(p => p.Name).SequenceEqual(
                from x in xElement.Descendants()
                where x.Name == "param"
                select x.Attribute("name").Value
            ) &&
            methodDefinition.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(
                xElement.Attribute("name").Value.Substring(
                    xElement.Attribute("name").Value.IndexOf('(') + 1,
                    xElement.Attribute("name").Value.Length - xElement.Attribute("name").Value.IndexOf('(') - 2
                ).Split(',')
            );

        private MethodDocumentation[] GetConstructorDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from m in typeDefinition.Methods
             where (m.IsConstructor &&
                   (m.Attributes & MethodAttributes.Public) == MethodAttributes.Public ||
                   (m.Attributes & MethodAttributes.Family) == MethodAttributes.Family) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("M:") &&
                        x.Attribute("name").Value.Contains($"{FullName}.#ctor") && 
                        IsSameOverload(m, x)
                    )
             select new MethodDocumentation(
                 m,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("M:") &&
                    x.Attribute("name").Value.Contains($"{FullName}.#ctor") &&
                    IsSameOverload(m, x)
             ))).ToArray();

        private FieldDocumentation[] GetFieldDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from f in typeDefinition.Fields
             where ((f.Attributes & FieldAttributes.Public) == FieldAttributes.Public ||
                   (f.Attributes & FieldAttributes.Family) == FieldAttributes.Family) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("F:") &&
                        x.Attribute("name").Value.EndsWith($"{FullName}.{f.Name}")
                    )
             select new FieldDocumentation(
                 f, 
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("F:") &&
                    x.Attribute("name").Value.EndsWith($"{FullName}.{f.Name}")
                 ), (from f2 in this.ReflectionTypeDefinition.GetFields()
                     where this.DeclaringAssembly.PEFile.Metadata.GetString(this.DeclaringAssembly.PEFile.Metadata.GetFieldDefinition(f2).Name) == f.Name
                     select f2).Single(),
                 this)
             ).ToArray();

        private MethodDocumentation[] GetMethodDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from m in typeDefinition.Methods
             where (!m.IsConstructor &&
                   (m.Attributes & MethodAttributes.Public) == MethodAttributes.Public ||
                   (m.Attributes & MethodAttributes.Family) == MethodAttributes.Family) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("M:") &&
                        x.Attribute("name").Value.Contains($"{FullName}.{m.Name}") &&
                        IsSameOverload(m, x)
                    )
             select new MethodDocumentation(
                 m,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("M:") &&
                    x.Attribute("name").Value.Contains($"{FullName}.{m.Name}") &&
                    IsSameOverload(m, x)
             ))).ToArray();

        private PropertyDocumentation[] GetPropertyDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from p in typeDefinition.Properties
             where (((p.GetMethod?.Attributes & MethodAttributes.Public) == MethodAttributes.Public ||
                   (p.GetMethod?.Attributes & MethodAttributes.Family) == MethodAttributes.Family) ||
                   ((p.SetMethod?.Attributes & MethodAttributes.Public) == MethodAttributes.Public ||
                   (p.SetMethod?.Attributes & MethodAttributes.Family) == MethodAttributes.Family)) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("P:") &&
                        x.Attribute("name").Value.EndsWith($"{FullName}.{p.Name}")
                    )
             select new PropertyDocumentation(
                 p,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("P:") &&
                    x.Attribute("name").Value.EndsWith($"{FullName}.{p.Name}")
                ), (from p2 in this.ReflectionTypeDefinition.GetProperties()
                    where this.DeclaringAssembly.PEFile.Metadata.GetString(this.DeclaringAssembly.PEFile.Metadata.GetPropertyDefinition(p2).Name) == p.Name
                    select p2).Single(),
                 this)
             ).ToArray();
    }
}
