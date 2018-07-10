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
        public TypeDocBuilder(TypeDoc type, Document document)
            : base(type, document)
        {
        }

        protected override void OnGenerate(MDDocument md)
        {
            AddTables(type, md);
        }

        private static void AddTables(TypeDoc type, MDDocument md)
        {
            var path = $"docs/{type.FullName.Replace(".", "/")}";

            AddTable("Constructors", $"{path}/Constructors", type.Constructors, md);

            AddTable("Properties", $"{path}/Properties", type.Properties, md);

            AddTable("Methods", $"{path}/Methods", type.Methods, md);

            AddTable("Fields", $"{path}/Fields", type.Fields, md);
        }

        private static void AddTable(string tableName, string path, IEnumerable<BaseDoc> items, MDDocument md)
        {
            if (!items.Any())
                return;

            md.AddElement(new MDH2
            {
                Text = tableName
            });

            var table = new MDTable();
            table.Header.Cells.Add(MDEmpty.Empty);
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
                row.Cells.Add(MDEmpty.Empty);
                row.Cells.Add(new MDLink
                {
                    Text = item.Name,
                    Url = HttpUtility.UrlEncode($"/{path}/{item.SafeName}.md")
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
            return $"{type.Name} Class";
        }
    }
}
