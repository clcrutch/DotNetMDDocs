using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DotNetMDDocs
{
    public class TypeDocBuilder : DocBuilder
    {
        private int currInheritanceLevel = 1;
        private readonly string rootName;

        public TypeDocBuilder(TypeDoc type, Document document, string rootName)
            : base(type, document)
        {
            this.rootName = rootName;
        }

        protected override void OnBeforeSyntax(MDDocument md)
        {
            md.AddElement(new MDH2
            {
                Text = "Inheritance Hierarchy"
            });

            RenderInheritanceLevel(type.InheritanceHierarchy, md);
        }

        private void RenderInheritanceLevel(InheritanceDoc inheritanceDoc, MDDocument md)
        {
            if (inheritanceDoc.BaseClass != null)
            {
                RenderInheritanceLevel(inheritanceDoc.BaseClass, md);
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < currInheritanceLevel; i++)
            {
                stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            currInheritanceLevel++;

            md.AddElement(new MDText
            {
                Text = $"{stringBuilder.ToString()}{inheritanceDoc.Namespace}.{inheritanceDoc.Name}"
            });
            md.AddElement(new MDText
            {
                Text = Environment.NewLine
            });
        }

        protected override void OnAfterSyntax(MDDocument md)
        {
            md.AddElement(new MDCode
            {
                Code = type.CodeSyntax,
                Language = "csharp"
            });
        }

        protected override void OnBeforeRemarks(MDDocument md)
        {
            AddTables(type, md);
        }

        private void AddTables(TypeDoc type, MDDocument md)
        {
            var path = $"{rootName}/{type.FullName.Replace(".", "/")}";

            AddTable("Constructors", $"{path}/Constructors", type.Constructors, md);

            AddTable("Properties", $"{path}/Properties", type.Properties, md);

            AddTable("Methods", $"{path}/Methods", type.Methods, md);

            AddTable("Fields", $"{path}/Fields", type.Fields, md);
        }

        private void AddTable(string tableName, string path, IEnumerable<BaseDoc> items, MDDocument md)
        {
            if (!items.Any())
                return;

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
                    Url = $"/{path}/{HttpUtility.UrlEncode(item.SafeName)}.md"
                });
                row.Cells.Add(new MDText
                {
                    Text = item.Summary
                });
                table.Rows.Add(row);
            }

            md.AddElement(table);
        }

        protected override string GetHeader()
        {
            return $"{type.Name} {(type.IsInterface ? "Interface" : "Class")}";
        }
    }
}
