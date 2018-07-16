// <copyright file="PropertyDoc.cs" company="Chris Crutchfield">
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

using System.Linq;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetMDDocs.XmlDocParser
{
    public class PropertyDoc : BaseDoc
    {
        public PropertyDoc(XElement xElement, string baseName, TypeDefinition typeDefinition)
            : base("P", xElement, $"{baseName}.")
        {
            var propertyDefinition = typeDefinition?.Properties.SingleOrDefault(p => p.Name == this.Name);

            this.CodeSyntax = this.GetCodeSyntax(propertyDefinition);
        }

        private string GetCodeSyntax(PropertyDefinition propertyDefinition)
        {
            if (propertyDefinition == null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(this.GetSyntaxAttributes(propertyDefinition));

            stringBuilder.Append("public ");
            stringBuilder.Append(propertyDefinition.PropertyType.Name);
            stringBuilder.Append(" ");
            stringBuilder.Append(propertyDefinition.Name);
            stringBuilder.Append(" { ");
            if (propertyDefinition.GetMethod?.IsPublic ?? false)
            {
                stringBuilder.Append("get; ");
            }

            if (propertyDefinition.SetMethod?.IsPublic ?? false)
            {
                stringBuilder.Append("set; ");
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}
