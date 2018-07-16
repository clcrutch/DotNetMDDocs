using DotNetMDDocs.XmlDocParser.Extensions;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class TypeDoc : BaseDoc
    {
        public bool IsInterface { get; private set; }

        public InheritanceDoc InheritanceHierarchy { get; private set; }
        public string Namespace { get; private set; }
        public IEnumerable<MethodDoc> Constructors { get; private set; }
        public IEnumerable<PropertyDoc> Properties { get; private set; }
        public IEnumerable<MethodDoc> Methods { get; private set; }
        public IEnumerable<FieldDoc> Fields { get; private set; }

        public string FullName => $"{Namespace}.{Name}";

        public TypeDoc(XElement xElement, XDocument xDocument, AssemblyDefinition assembly)
            : base("T", xElement, string.Empty)
        {
            Namespace = xElement.Attribute("name").Value.Substring("T:".Length);
            Namespace = Namespace.Substring(0, Namespace.LastIndexOf('.'));

            Name = Name.Replace($"{Namespace}.", string.Empty);

            var type = assembly.MainModule.GetType(Namespace, Name);

            Constructors = GetConstructors(xDocument);
            Properties = GetProperties(xDocument, type);
            Methods = GetMethods(xDocument);
            Fields = GetFields(xDocument);

            InheritanceHierarchy = GetInheritanceHierarchy(type);
            CodeSyntax = GetCodeSyntax(type);

            IsInterface = type?.IsInterface ?? false;
        }

        private InheritanceDoc GetInheritanceHierarchy(TypeDefinition type)
        {
            InheritanceDoc baseClass = null;
            if (type != null && type.BaseType != null)
                baseClass = GetInheritanceHierarchy(type.BaseType.Resolve());

            var @return = new InheritanceDoc
            {
                BaseClass = baseClass,
                Name = type?.Name ?? Name,
                Namespace = type?.Namespace ?? Namespace
            };

            return @return;
        }

        private string GetCodeSyntax(TypeDefinition type)
        {
            if (type == null)
                return string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(GetSyntaxAttributes(type));

            stringBuilder.Append($"public {(type.IsInterface ? "interface" : "class")} {type.Name}");

            bool hasBaseType = type.BaseType != null && type.BaseType.FullName != "System.Object";

            if (hasBaseType || type.HasInterfaces)
            {
                stringBuilder.Append(" : ");

                bool hasAppendedType = false;
                if (hasBaseType)
                {
                    stringBuilder.Append(type.BaseType.DisplayName());
                    hasAppendedType = true;
                }

                foreach (var @interface in type.Interfaces)
                {
                    if (hasAppendedType)
                        stringBuilder.Append(", ");

                    stringBuilder.Append(@interface.InterfaceType.DisplayName());
                }
            }

            return stringBuilder.ToString();
        }

        private IEnumerable<MethodDoc> GetConstructors(XDocument xDocument)
        {
            return (from m in GetMembers("M", xDocument)
                    where m.Attribute("name").Value.Contains("#ctor")
                    select new MethodDoc(m, FullName)).ToArray();
        }

        private IEnumerable<PropertyDoc> GetProperties(XDocument xDocument, TypeDefinition typeDefinition)
        {
            return (from m in GetMembers("P", xDocument)
                    select new PropertyDoc(m, FullName, typeDefinition)).ToArray();
        }

        private IEnumerable<MethodDoc> GetMethods(XDocument xDocument)
        {
            return (from m in GetMembers("M", xDocument)
                    where !m.Attribute("name").Value.Contains("#ctor")
                    select new MethodDoc(m, FullName)).ToArray();
        }

        private IEnumerable<FieldDoc> GetFields(XDocument xDocument)
        {
            return (from m in GetMembers("F", xDocument)
                    select new FieldDoc(m, FullName)).ToArray();
        }

        private IEnumerable<XElement> GetMembers(string identifier, XDocument xDocument)
        {
            var members = (from e in xDocument.Root.Elements()
                           where e.Name == "members"
                           select e).Single();

            return from e in members.Elements()
                   where e.Name == "member" && e.Attribute("name").Value.StartsWith($"{identifier}:{FullName}.")
                   select e;
        }
    }
}
