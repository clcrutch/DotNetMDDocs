using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDTable : IMDElement
    {
        public MDTableRow Header { get; } = new MDTableRow();
        public List<MDTableRow> Rows { get; } = new List<MDTableRow>();

        public string Generate()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("|");
            foreach (var header in Header.Cells)
            {
                stringBuilder.Append(header.Generate());
                stringBuilder.Append("|");
            }
            stringBuilder.AppendLine();

            stringBuilder.Append("|");
            foreach (var header in Header.Cells)
            {
                stringBuilder.Append("---|");
            }
            stringBuilder.AppendLine();

            foreach (var row in Rows)
            {
                stringBuilder.Append("|");
                foreach (var cell in row.Cells)
                {
                    stringBuilder.Append(cell.Generate());
                    stringBuilder.Append("|");
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
