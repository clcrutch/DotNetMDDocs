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
using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;

namespace DotNetMDDocs
{
    public class TypeDocBuilder : DocBuilder
    {
        private readonly string rootName;
        private int currInheritanceLevel = 1;

        public TypeDocBuilder(TypeDoc type, Document document, string rootName)
            : base(type, type, document)
        {
            this.rootName = rootName;
        }

        protected override string GetHeader()
        {
            return $"{this.Type.Name} {(this.Type.IsInterface ? "Interface" : "Class")}";
        }

        protected override void OnBeforeSyntax(MDDocument md)
        {
            md.AddElement(new MDH2
            {
                Text = "Inheritance Hierarchy"
            });

            this.RenderInheritanceLevel(this.Type.InheritanceHierarchy, md);
        }

        protected override void OnBeforeRemarks(MDDocument md)
        {
            this.AddTables(this.Type, md);
        }

        private void RenderInheritanceLevel(InheritanceDoc inheritanceDoc, MDDocument md)
        {
            if (inheritanceDoc.BaseClass != null)
            {
                this.RenderInheritanceLevel(inheritanceDoc.BaseClass, md);
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < this.currInheritanceLevel; i++)
            {
                stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }

            this.currInheritanceLevel++;

            md.AddElement(new MDText
            {
                Text = $"{stringBuilder.ToString()}{inheritanceDoc.Namespace}.{inheritanceDoc.Name}"
            });
            md.AddElement(new MDText
            {
                Text = $"{Environment.NewLine}{Environment.NewLine}"
            });
        }

        private void AddTables(TypeDoc type, MDDocument md)
        {
            var path = $"{this.rootName}/{type.FullName.Replace(".", "/")}";

            this.AddTable("Constructors", $"{path}/Constructors", type.Constructors, md);

            this.AddTable("Properties", $"{path}/Properties", type.Properties, md);

            this.AddTable("Methods", $"{path}/Methods", type.Methods, md);

            this.AddTable("Fields", $"{path}/Fields", type.Fields, md);
        }

        private void AddTable(string tableName, string path, IEnumerable<BaseDoc> items, MDDocument md)
        {
            if (!items.Any())
            {
                return;
            }

            md.AddElement(new MDH2
            {
                Text = tableName
            });

            var table = new MDTable();
            table.Header.Cells.Add(new MDText
            {
                Text = "Name"
            });
            table.Header.Cells.Add(new MDText
            {
                Text = "Description"
            });

            foreach (var item in items)
            {
                var row = new MDTableRow();
                row.Cells.Add(new MDLink
                {
                    Text = item.Name,
                    Url = $"/{path}/{HttpUtility.UrlEncode(item.SafeName).Replace("+", "%20")}.md"
                });
                row.Cells.Add(new MDText
                {
                    Text = item.Summary
                });
                table.Rows.Add(row);
            }

            md.AddElement(table);
        }
    }
}
