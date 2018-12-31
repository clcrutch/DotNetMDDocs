// <copyright file="TypeDocumentation.cs" company="Chris Crutchfield">
// Copyright (C) 2017  Chris Crutchfield
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see &lt;http://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using Mono.Cecil;

using MethodDefinition = Mono.Cecil.MethodDefinition;
using TypeAttributes = System.Reflection.TypeAttributes;
using TypeDefinition = Mono.Cecil.TypeDefinition;

namespace DotNetDocs
{
    /// <summary>
    /// Parses a type.
    /// </summary>
    public class TypeDocumentation : DocumentationBase
    {
        private static Dictionary<string, TypeDocumentation> typeMap = new Dictionary<string, TypeDocumentation>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDocumentation"/> class.
        /// </summary>
        /// <param name="typeDefinition">The <see cref="TypeDefinition"/> which to document.</param>
        /// <param name="xElement">The XML element representing the XML comments for the current type.</param>
        /// <param name="declaringAssembly">Type assembly which declares this type.</param>
        protected internal TypeDocumentation(TypeDefinition typeDefinition, XElement xElement, AssemblyDocumentation declaringAssembly)
            : base(typeDefinition, xElement, null)
        {
            this.DeclaringAssembly = declaringAssembly;

            var typeDefs = from t in declaringAssembly.PEFile.Metadata.TypeDefinitions
                           select declaringAssembly.PEFile.Metadata.GetTypeDefinition(t);

            this.ReflectionTypeDefinition = (from t in typeDefs
                                             where declaringAssembly.PEFile.Metadata.GetString(t.Namespace) == this.Namespace &&
                                                declaringAssembly.PEFile.Metadata.GetString(t.Name) == this.Name
                                             select t).Single();

            this.ConstructorDocumentations = this.GetConstructorDocumentations(typeDefinition, xElement.Document);
            this.FieldDocumentations = this.GetFieldDocumentations(typeDefinition, xElement.Document);
            this.PropertyDocumentations = this.GetPropertyDocumentations(typeDefinition, xElement.Document);
            this.MethodDocumentations = this.GetMethodDocumentations(typeDefinition, xElement.Document);

            var name = new FullTypeName(this.FullName);
            this.Declaration = this.DeclaringAssembly.Decompiler.DecompileTypeAsString(name);
            this.Declaration = this.Declaration.Substring(this.Declaration.IndexOf('{') + 1);
            this.Declaration = this.Declaration.Substring(0, this.Declaration.IndexOf('{')).Trim();

            typeMap.Add(typeDefinition.FullName, this);
        }

        public TypeDocumentation BaseType => typeMap[this.TypeDefinition.BaseType.FullName];

        /// <summary>
        /// Gets the underlying <see cref="TypeDefinition"/>.
        /// </summary>
        public TypeDefinition TypeDefinition => (TypeDefinition)MemberDefinition;

        /// <summary>
        /// Gets a list of constructors for the current type.
        /// </summary>
        public MethodDocumentation[] ConstructorDocumentations { get; private set; }

        /// <summary>
        /// Gets the assembly that declares the current type.
        /// </summary>
        public AssemblyDocumentation DeclaringAssembly { get; private set; }

        /// <summary>
        /// Gets a list of fields for the current type.
        /// </summary>
        public FieldDocumentation[] FieldDocumentations { get; private set; }

        /// <summary>
        /// Gets a list of methods for the current type.
        /// </summary>
        public MethodDocumentation[] MethodDocumentations { get; private set; }

        /// <summary>
        /// Gets the namespace for the current type.
        /// </summary>
        public string Namespace => TypeDefinition.Namespace;

        /// <summary>
        /// Gets a list of the properties for the current type.
        /// </summary>
        public PropertyDocumentation[] PropertyDocumentations { get; private set; }

        /// <summary>
        /// Gets the underlying <see cref="System.Reflection.Metadata.TypeDefinition"/> for the type.
        /// </summary>
        public System.Reflection.Metadata.TypeDefinition ReflectionTypeDefinition { get; private set; }

        /// <summary>
        /// Gets the <see cref="TypeAttributes"/> for the current type.
        /// </summary>
        public TypeAttributes TypeAttributes => (TypeAttributes)TypeDefinition.Attributes;

        private MethodDocumentation[] GetConstructorDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from m in typeDefinition.Methods
             where ((m.IsConstructor &&
                   (m.Attributes & MethodAttributes.Public) == MethodAttributes.Public) ||
                   (m.Attributes & MethodAttributes.Family) == MethodAttributes.Family) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("M:") &&
                        x.Attribute("name").Value.Contains($"{this.FullName}.#ctor") &&
                        this.IsSameOverload(m, x))
             select new MethodDocumentation(
                 m,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("M:") &&
                    x.Attribute("name").Value.Contains($"{this.FullName}.#ctor") &&
                    this.IsSameOverload(m, x)), (from m2 in this.ReflectionTypeDefinition.GetMethods()
                                                 where this.DeclaringAssembly.PEFile.Metadata.GetString(this.DeclaringAssembly.PEFile.Metadata.GetMethodDefinition(m2).Name) == m.Name
                                                    & this.IsSameOverload(m2, m)
                                                 select m2).First(),
                 this))
            .ToArray();

        private FieldDocumentation[] GetFieldDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from f in typeDefinition.Fields
             where ((f.Attributes & FieldAttributes.Public) == FieldAttributes.Public ||
                   (f.Attributes & FieldAttributes.Family) == FieldAttributes.Family) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("F:") &&
                        x.Attribute("name").Value.EndsWith($"{this.FullName}.{f.Name}"))
             select new FieldDocumentation(
                 f,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("F:") &&
                    x.Attribute("name").Value.EndsWith($"{this.FullName}.{f.Name}")), (from f2 in this.ReflectionTypeDefinition.GetFields()
                                                                                       where this.DeclaringAssembly.PEFile.Metadata.GetString(
                                                                                           this.DeclaringAssembly.PEFile.Metadata.GetFieldDefinition(f2).Name) == f.Name
                                                                                       select f2).Single(),
                 this))
             .ToArray();

        private MethodDocumentation[] GetMethodDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from m in typeDefinition.Methods
             where ((!m.IsConstructor &&
                   (m.Attributes & MethodAttributes.Public) == MethodAttributes.Public) ||
                   (m.Attributes & MethodAttributes.Family) == MethodAttributes.Family) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("M:") &&
                        x.Attribute("name").Value.Contains($"{this.FullName}.{m.Name}") &&
                        this.IsSameOverload(m, x))
             select new MethodDocumentation(
                 m,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("M:") &&
                    x.Attribute("name").Value.Contains($"{this.FullName}.{m.Name}") &&
                    this.IsSameOverload(m, x)), (from m2 in this.ReflectionTypeDefinition.GetMethods()
                                            where this.DeclaringAssembly.PEFile.Metadata.GetString(this.DeclaringAssembly.PEFile.Metadata.GetMethodDefinition(m2).Name) == m.Name &
                                                this.IsSameOverload(m2, m)
                                            select m2).First(),
                 this))
             .ToArray();

        private PropertyDocumentation[] GetPropertyDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from p in typeDefinition.Properties
             where (((p.GetMethod?.Attributes & MethodAttributes.Public) == MethodAttributes.Public ||
                   (p.GetMethod?.Attributes & MethodAttributes.Family) == MethodAttributes.Family) ||
                   ((p.SetMethod?.Attributes & MethodAttributes.Public) == MethodAttributes.Public ||
                   (p.SetMethod?.Attributes & MethodAttributes.Family) == MethodAttributes.Family)) &&
                    xDocument.Descendants().Any(x =>
                        x.Name == "member" &&
                        x.Attribute("name").Value.StartsWith("P:") &&
                        x.Attribute("name").Value.EndsWith($"{this.FullName}.{p.Name}"))
             select new PropertyDocumentation(
                 p,
                 xDocument.Descendants().Single(x =>
                    x.Name == "member" &&
                    x.Attribute("name").Value.StartsWith("P:") &&
                    x.Attribute("name").Value.EndsWith($"{this.FullName}.{p.Name}")), (from p2 in this.ReflectionTypeDefinition.GetProperties()
                    where this.DeclaringAssembly.PEFile.Metadata.GetString(this.DeclaringAssembly.PEFile.Metadata.GetPropertyDefinition(p2).Name) == p.Name
                    select p2).Single(),
                 this))
             .ToArray();

        private bool IsSameOverload(MethodDefinition methodDefinition, XElement xElement) =>
            methodDefinition.HasParameters == xElement.Descendants().Any(x => x.Name == "param") &&
            methodDefinition.Parameters.Count == xElement.Descendants().Count(x => x.Name == "param") &&
            methodDefinition.Parameters.Select(p => p.Name).SequenceEqual(
                from x in xElement.Descendants()
                where x.Name == "param"
                select x.Attribute("name").Value) &&
            methodDefinition.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(
                xElement.Attribute("name").Value.Substring(
                    xElement.Attribute("name").Value.IndexOf('(') + 1,
                    xElement.Attribute("name").Value.Length - xElement.Attribute("name").Value.IndexOf('(') - 2)
                .Split(','));

        private bool IsSameOverload(MethodDefinitionHandle methodDefinitionHandle, MethodDefinition methodDefinition)
        {
            var m2 = this.DeclaringAssembly.PEFile.Metadata.GetMethodDefinition(methodDefinitionHandle);
            var parameterHandles = m2.GetParameters();

            var parameters = (from p in parameterHandles
                              select this.DeclaringAssembly.PEFile.Metadata.GetParameter(p)).ToArray();

            return methodDefinition.HasParameters && parameterHandles.Any() &&
                methodDefinition.Parameters.Count == parameterHandles.Count &&
                (from p in parameters
                 select this.DeclaringAssembly.PEFile.Metadata.GetString(p.Name)).SequenceEqual(from p in methodDefinition.Parameters
                                                                                           select p.Name);
        }
    }
}
