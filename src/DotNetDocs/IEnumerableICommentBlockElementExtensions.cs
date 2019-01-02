// <copyright file="IEnumerableICommentBlockElementExtensions.cs" company="Chris Crutchfield">
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
using DotNetDocs.CommentBlockElements;

namespace DotNetDocs
{
    /// <summary>
    /// Extension methods for IEnumerable(ICommentBlockElement).
    /// </summary>
    public static class IEnumerableICommentBlockElementExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="IEnumerable{ICommentBlockElement}"/> to a string.
        /// </summary>
        /// <param name="this">The comment block list to convert to a string.</param>
        /// <returns>A string representing the comment blocks.</returns>
        public static string ConvertToString(this IEnumerable<ICommentBlockElement> @this)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in @this)
            {
                if (item is StringCommentBlockElement)
                {
                    stringBuilder.Append(((StringCommentBlockElement)item).Content);
                }
                else if (item is SeeCommentBlockElement)
                {
                    var see = item as SeeCommentBlockElement;
                    stringBuilder.Append(see.TypeName);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
