// <copyright file="TypeDoc.cs" company="Chris Crutchfield">
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
using System.Text;
using System.Xml.Linq;
using DotNetMDDocs.XmlDocParser.Extensions;
using Mono.Cecil;

namespace DotNetMDDocs.XmlDocParser
{
    public class TypeDoc : BaseDoc
    {
        public TypeDoc(XElement xElement, XDocument xDocument, AssemblyDefinition assembly)
            : base("T", xElement, string.Empty)
        {
            this.Namespace = xElement.Attribute("name").Value.Substring("T:".Length);
            this.Namespace = this.Namespace.Substring(0, this.Namespace.LastIndexOf('.'));

            this.Name = this.Name.Replace($"{this.Namespace}.", string.Empty);

            var type = assembly.MainModule.GetType(this.Namespace, this.Name);

            this.Constructors = this.GetConstructors(xDocument);
            this.Properties = this.GetProperties(xDocument, type);
            this.Methods = this.GetMethods(xDocument);
            this.Fields = this.GetFields(xDocument);

            this.InheritanceHierarchy = this.GetInheritanceHierarchy(type);
            this.CodeSyntax = this.GetCodeSyntax(type);

            this.IsInterface = type?.IsInterface ?? false;
        }

        public bool IsInterface { get; private set; }

        public InheritanceDoc InheritanceHierarchy { get; private set; }

        public string Namespace { get; private set; }

        public IEnumerable<MethodDoc> Constructors { get; private set; }

        public IEnumerable<PropertyDoc> Properties { get; private set; }

        public IEnumerable<MethodDoc> Methods { get; private set; }

        public IEnumerable<FieldDoc> Fields { get; private set; }

        public string FullName => $"{this.Namespace}.{this.Name}";

        private InheritanceDoc GetInheritanceHierarchy(TypeDefinition type)
        {
            InheritanceDoc baseClass = null;
            if (type != null && type.BaseType != null)
            {
                baseClass = this.GetInheritanceHierarchy(type.BaseType.Resolve());
            }

            var @return = new InheritanceDoc
            {
                BaseClass = baseClass,
                Name = type?.Name ?? this.Name,
                Namespace = type?.Namespace ?? this.Namespace
            };

            return @return;
        }

        private string GetCodeSyntax(TypeDefinition type)
        {
            if (type == null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(this.GetSyntaxAttributes(type));

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
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(@interface.InterfaceType.DisplayName());
                }
            }

            return stringBuilder.ToString();
        }

        private IEnumerable<MethodDoc> GetConstructors(XDocument xDocument)
        {
            return (from m in this.GetMembers("M", xDocument)
                    where m.Attribute("name").Value.Contains("#ctor")
                    select new MethodDoc(m, this.FullName)).ToArray();
        }

        private IEnumerable<PropertyDoc> GetProperties(XDocument xDocument, TypeDefinition typeDefinition)
        {
            return (from m in this.GetMembers("P", xDocument)
                    select new PropertyDoc(m, this.FullName, typeDefinition)).ToArray();
        }

        private IEnumerable<MethodDoc> GetMethods(XDocument xDocument)
        {
            return (from m in this.GetMembers("M", xDocument)
                    where !m.Attribute("name").Value.Contains("#ctor")
                    select new MethodDoc(m, this.FullName)).ToArray();
        }

        private IEnumerable<FieldDoc> GetFields(XDocument xDocument)
        {
            return (from m in this.GetMembers("F", xDocument)
                    select new FieldDoc(m, this.FullName)).ToArray();
        }

        private IEnumerable<XElement> GetMembers(string identifier, XDocument xDocument)
        {
            var members = (from e in xDocument.Root.Elements()
                           where e.Name == "members"
                           select e).Single();

            return from e in members.Elements()
                   where e.Name == "member" && e.Attribute("name").Value.StartsWith($"{identifier}:{this.FullName}.")
                   select e;
        }
    }
}
