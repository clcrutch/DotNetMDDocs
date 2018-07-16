// <copyright file="BaseDoc.cs" company="Chris Crutchfield">
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

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetMDDocs.XmlDocParser
{
    public abstract class BaseDoc
    {
        private string safeName;

        public BaseDoc(string identifier, XElement xElement, string baseName)
        {
            this.Name = xElement.Attribute("name").Value.Substring($"{identifier}:{baseName}".Length);

            this.Summary = (from e in xElement.Elements()
                            where e.Name == "summary"
                            select e.Value
                                .Replace(Environment.NewLine, " ")
                                .Replace("\n", " ")
                                .Trim()).SingleOrDefault();

            this.Remarks = (from e in xElement.Elements()
                            where e.Name == "remarks"
                            select e.Value
                                .Replace(Environment.NewLine, " ")
                                .Replace("\n", " ")
                                .Trim()).SingleOrDefault();
        }

        public virtual string Name { get; protected set; }

        public string Summary { get; protected set; }

        public string Remarks { get; protected set; }

        public string CodeSyntax { get; protected set; }

        public string SafeName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.safeName))
                {
                    return this.safeName;
                }

                this.safeName = this.Name;

                // Windows breaks if we don't keep the file name short.
                if (this.safeName.Length > 50)
                {
                    int hash = 0;
                    foreach (var c in this.safeName.ToCharArray())
                    {
                        hash += c;
                    }

                    hash %= 99999;

                    // Take the first part of the string so that we can recognize it.
                    // Hope that the hash is enough to make it unique.
                    this.safeName = $"{this.safeName.Substring(0, 45)}{hash}";
                }

                foreach (var invalid in Path.GetInvalidFileNameChars())
                {
                    this.safeName = this.safeName.Replace(invalid, '_');
                }

                this.safeName = this.safeName.Replace('(', '_');
                this.safeName = this.safeName.Replace(')', '_');

                return this.safeName;
            }
        }

        protected virtual void Initialize(IMemberDefinition memberDefinition)
        {
            if (memberDefinition == null)
            {
                return;
            }

            this.CodeSyntax = this.GetCodeSyntax(memberDefinition);
        }

        protected virtual string GetCodeSyntax(IMemberDefinition memberDefinition)
        {
            var stringBuilder = new StringBuilder();

            var attributes = this.GetSyntaxAttributes(memberDefinition);
            var declaration = this.GetSyntaxDeclaration(memberDefinition);

            if (!string.IsNullOrWhiteSpace(attributes))
            {
                stringBuilder.AppendLine(attributes);
            }

            if (!string.IsNullOrWhiteSpace(declaration))
            {
                stringBuilder.AppendLine(declaration);
            }

            return stringBuilder.ToString().Trim();
        }

        protected virtual string GetSyntaxAttributes(IMemberDefinition memberDefinition)
        {
            var stringBuilder = new StringBuilder();

            foreach (var attribute in memberDefinition.CustomAttributes)
            {
                stringBuilder.Append($"[{attribute.AttributeType.Name}");

                // if (attribute.HasConstructorArguments)
                // {
                //    var ctorArgs = (from c in attribute.ConstructorArguments
                //                    select c.ToCodeString()).ToArray();

                // stringBuilder.Append($"({string.Join(", ", ctorArgs)})");
                // }
                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

        protected virtual string GetSyntaxDeclaration(IMemberDefinition memberDefinition)
        {
            return string.Empty;
        }
    }
}
