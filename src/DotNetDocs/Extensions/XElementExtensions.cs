// <copyright file="XElementExtensions.cs" company="Chris Crutchfield">
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

using System.Xml;
using System.Xml.Linq;

namespace DotNetDocs.Extensions
{
    /// <summary>
    /// Collection of extension methods to XElement.
    /// </summary>
    internal static class XElementExtensions
    {
        /// <summary>
        /// Convert the current <see cref="XElement"/> to a <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="this">The <see cref="XElement"/> to convert.</param>
        /// <returns>The resulting <see cref="XmlElement"/>.</returns>
        public static XmlElement ToXmlElement(this XElement @this)
        {
            if (@this == null)
            {
                return null;
            }

            var doc = new XmlDocument();
            doc.LoadXml(@this.ToString().TrimAndCombine(" "));
            return doc.DocumentElement;
        }
    }
}
