// <copyright file="MDGroup.cs" company="Chris Crutchfield">
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
    /// Represents a group of markdown elements.
    /// </summary>
    public class MDGroup : IMDElement
    {
        private readonly List<IMDElement> elements = new List<IMDElement>();

        /// <summary>
        /// Adds an element to the group.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void AddElement(IMDElement element)
        {
            this.elements.Add(element);
        }

        /// <summary>
        /// Adds a range of elements to the group.
        /// </summary>
        /// <param name="elements">The elements to add.</param>
        public void AddElementRange(IEnumerable<IMDElement> elements)
        {
            this.elements.AddRange(elements);
        }

        /// <inheritdoc />
        public string Generate()
        {
            var stringBuilder = new StringBuilder();

            foreach (var element in this.elements)
            {
                stringBuilder.Append(element?.Generate());
            }

            return stringBuilder.ToString();
        }
    }
}
