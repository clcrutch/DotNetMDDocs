// <copyright file="FieldDoc.cs" company="Chris Crutchfield">
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
    public class FieldDoc : BaseDoc
    {
        public FieldDoc(XElement xElement, string baseName, TypeDefinition typeDefinition)
            : base("F", xElement, $"{baseName}.")
        {
            var fieldDefinition = typeDefinition?.Fields.SingleOrDefault(p => p.Name == this.Name);

            this.Initialize(fieldDefinition);
        }

        protected override string GetSyntaxDeclaration(IMemberDefinition memberDefinition)
        {
            if (memberDefinition is FieldDefinition fieldDefinition)
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append("private ");
                stringBuilder.Append(fieldDefinition.FieldType.Name);
                stringBuilder.Append(" ");
                stringBuilder.Append(fieldDefinition.Name);
                stringBuilder.Append(";");

                return stringBuilder.ToString();
            }

            return string.Empty;
        }
    }
}
