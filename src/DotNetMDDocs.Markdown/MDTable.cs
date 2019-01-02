// <copyright file="MDTable.cs" company="Chris Crutchfield">
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
using System.Text;

namespace DotNetMDDocs.Markdown
{
    /// <summary>
    /// Represents a markdown table element.
    /// </summary>
    public class MDTable : IMDElement
    {
        /// <summary>
        /// Gets the header row of the table.
        /// </summary>
        public MDTableRow Header { get; } = new MDTableRow();

        /// <summary>
        /// Gets the rows of the table.
        /// </summary>
        public List<MDTableRow> Rows { get; } = new List<MDTableRow>();

        /// <inheritdoc />
        public string Generate()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("|");
            foreach (var header in this.Header.Cells)
            {
                stringBuilder.Append(header.Generate());
                stringBuilder.Append("|");
            }

            stringBuilder.AppendLine();

            stringBuilder.Append("|");
            foreach (var header in this.Header.Cells)
            {
                stringBuilder.Append("---|");
            }

            stringBuilder.AppendLine();

            foreach (var row in this.Rows)
            {
                stringBuilder.Append("|");
                foreach (var cell in row.Cells)
                {
                    stringBuilder.Append(cell?.Generate());
                    stringBuilder.Append("|");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
