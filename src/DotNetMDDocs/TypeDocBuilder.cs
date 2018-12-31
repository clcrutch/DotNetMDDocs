// <copyright file="TypeDocBuilder.cs" company="Chris Crutchfield">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetDocs;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;
using Mono.Cecil;

namespace DotNetMDDocs
{
    public class TypeDocBuilder : DocBuilder
    {
        public const string SPACE = "&nbsp;";

        private readonly string rootName;
        private int currInheritanceLevel = 1;

        public TypeDocBuilder(TypeDocumentation typeDocumentation, AssemblyDocumentation assemblyDocumentation, string rootName)
            : base(typeDocumentation, typeDocumentation, assemblyDocumentation)
        {
            this.rootName = rootName;
        }

        protected string GetDeclarationType()
        {
            if ((this.TypeDocumentation.TypeAttributes & System.Reflection.TypeAttributes.Interface) == System.Reflection.TypeAttributes.Interface)
            {
                return "Interface";
            }
            else if ((this.TypeDocumentation.TypeAttributes & System.Reflection.TypeAttributes.Class) == System.Reflection.TypeAttributes.Class)
            {
                return "Class";
            }
            else if (this.TypeDocumentation.TypeDefinition.IsEnum)
            {
                return "Enum";
            }

            return "Unknown";
        }

        protected override string GetHeader()
        {
            return $"{this.TypeDocumentation.Name} {this.GetDeclarationType()}";
        }

        protected override void OnBeforeSyntax(MDDocument md)
        {
            md.AddElement(new MDH2
            {
                Text = "Inheritance Hierarchy",
            });

            this.RenderInheritanceLevel(this.TypeDocumentation.TypeDefinition, md);
        }

        protected override void OnBeforeRemarks(MDDocument md)
        {
            this.AddTables(this.TypeDocumentation, md);
        }

        private void RenderInheritanceLevel(TypeReference typeReference, MDDocument md)
        {
            var typeDefinition = typeReference.Resolve();

            if (typeDefinition.BaseType != null)
            {
                this.RenderInheritanceLevel(typeDefinition.BaseType, md);
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < this.currInheritanceLevel; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    stringBuilder.Append(SPACE);
                }
            }

            this.currInheritanceLevel++;

            var typeName = $"{stringBuilder.ToString()}{typeDefinition.FullName}";
            md.AddElement(new MDLink
            {
                Url = UrlHelper.GetType(typeName),
                Text = typeName,
            });
            md.AddElement(new MDText
            {
                Text = $"{Environment.NewLine}{Environment.NewLine}",
            });
        }

        private void AddTables(TypeDocumentation type, MDDocument md)
        {
            var path = $"{this.rootName}/{type.FullName.Replace(".", "/")}";

            this.AddTable("Constructors", $"{path}/Constructors", type.ConstructorDocumentations, md);

            this.AddTable("Properties", $"{path}/Properties", type.PropertyDocumentations, md);

            this.AddTable("Methods", $"{path}/Methods", type.MethodDocumentations, md);

            this.AddTable("Fields", $"{path}/Fields", type.FieldDocumentations, md);
        }

        private void AddTable(string tableName, string path, IEnumerable<DocumentationBase> items, MDDocument md)
        {
            if (!items.Any())
            {
                return;
            }

            md.AddElement(new MDH2
            {
                Text = tableName,
            });

            var table = new MDTable();
            table.Header.Cells.Add(new MDText
            {
                Text = "Name",
            });
            table.Header.Cells.Add(new MDText
            {
                Text = "Description",
            });

            foreach (var item in items)
            {
                var row = new MDTableRow();
                row.Cells.Add(new MDLink
                {
                    Text = item.Name,
                    Url = $"/{path}/{HttpUtility.UrlEncode(item.GetSafeName()).Replace("+", "%20")}.md",
                });
                row.Cells.Add(item.Summary?.ConvertToMDGroup());
                table.Rows.Add(row);
            }

            md.AddElement(table);
        }
    }
}
