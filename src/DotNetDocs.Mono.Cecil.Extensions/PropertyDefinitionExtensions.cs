// <copyright file="PropertyDefinitionExtensions.cs" company="Chris Crutchfield">
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
using System.Xml.Linq;
using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;

namespace DotNetDocs.Mono.Cecil.Extensions
{
    /// <summary>
    /// Extensions to <see cref="PropertyDefinition"/>.
    /// </summary>
    public static class PropertyDefinitionExtensions
    {
        /// <summary>
        /// Convert to <see cref="System.Reflection.Metadata.EntityHandle"/> to use with ICSharpCode.Decompiler.
        /// </summary>
        /// <param name="propertyDefinition">The <see cref="PropertyDefinition"/> to convert to an entity handle.</param>
        /// <param name="peFile">The <see cref="PEFile"/> which to find <paramref name="propertyDefinition"/> in.</param>
        /// <returns>A reference to the entity handle for <paramref name="propertyDefinition"/>.</returns>
        public static System.Reflection.Metadata.PropertyDefinitionHandle? ToEntityHandle(this PropertyDefinition propertyDefinition, PEFile peFile)
        {
            if (peFile == null)
            {
                return null;
            }

            var handle = propertyDefinition.DeclaringType.ToEntityHandle(peFile);
            if (!handle.HasValue)
            {
                return null;
            }

            var reflectionTypeDefinition = peFile.Metadata.GetTypeDefinition(handle.Value);

            return (from p in reflectionTypeDefinition.GetProperties()
                    where peFile.Metadata.GetString(peFile.Metadata.GetPropertyDefinition(p).Name) == propertyDefinition.Name
                    select p).Single();
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> for <paramref name="propertyDefinition"/>.
        /// </summary>
        /// <param name="propertyDefinition">The <see cref="PropertyDefinition"/> to find the <see cref="XElement"/> for.</param>
        /// <param name="xDocument">The document to find <paramref name="propertyDefinition"/> in.</param>
        /// <returns>The document <see cref="XElement"/> which represents <paramref name="propertyDefinition"/>.</returns>
        public static XElement ToXElement(this PropertyDefinition propertyDefinition, XDocument xDocument)
        {
            if (propertyDefinition == null || xDocument == null)
            {
                return null;
            }

            var memberNodes = from d in xDocument.Descendants()
                              where d.Name == "member"
                              select d;

            return memberNodes.SingleOrDefault(m => m.Attribute("name")?.Value == $"P:{propertyDefinition.DeclaringType.FullName}.{propertyDefinition.Name}");
        }
    }
}
