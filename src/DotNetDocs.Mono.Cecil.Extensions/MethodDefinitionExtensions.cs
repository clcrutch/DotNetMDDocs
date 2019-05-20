// <copyright file="MethodDefinitionExtensions.cs" company="Chris Crutchfield">
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
using DotNetDocs.Reflection.Metadata.Extensions;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;

namespace DotNetDocs.Mono.Cecil.Extensions
{
    /// <summary>
    /// Extensions to <see cref="MethodDefinition"/>.
    /// </summary>
    public static class MethodDefinitionExtensions
    {
        /// <summary>
        /// Convert to <see cref="System.Reflection.Metadata.EntityHandle"/> to use with ICSharpCode.Decompiler.
        /// </summary>
        /// <param name="methodDefinition">The <see cref="MethodDefinition"/> to convert to an entity handle.</param>
        /// <param name="peFile">The <see cref="PEFile"/> which to find <paramref name="methodDefinition"/> in.</param>
        /// <param name="decompiler">An instance of <see cref="CSharpDecompiler"/> used for finding method parameters.</param>
        /// <returns>A reference to the entity handle for <paramref name="methodDefinition"/>.</returns>
        public static System.Reflection.Metadata.MethodDefinitionHandle? ToEntityHandle(this MethodDefinition methodDefinition, PEFile peFile, CSharpDecompiler decompiler)
        {
            if (peFile == null || decompiler == null)
            {
                return null;
            }

            var methodString = methodDefinition.ToMethodString();

            var handle = methodDefinition.DeclaringType.ToEntityHandle(peFile);
            if (!handle.HasValue)
            {
                return null;
            }

            var reflectionTypeDefinition = peFile.Metadata.GetTypeDefinition(handle.Value);

            return reflectionTypeDefinition.GetMethods().SingleOrDefault(m => m.ToMethodString(peFile, decompiler) == methodString);
        }

        /// <summary>
        /// Gets a string representing the <see cref="MethodDefinition"/>.
        /// </summary>
        /// <param name="methodDefinition">The <see cref="MethodDefinition"/> to represent as a string.</param>
        /// <returns>A string repsentation of <paramref name="methodDefinition"/>.</returns>
        public static string ToMethodString(this MethodDefinition methodDefinition)
        {
            if (methodDefinition == null)
            {
                return null;
            }

            var parameters = from p in methodDefinition.Parameters
                             select p.ParameterType.ToTypeString();

            return $"{methodDefinition.DeclaringType.FullName}.{(methodDefinition.IsConstructor ? "#ctor" : methodDefinition.Name)}{(methodDefinition.HasGenericParameters ? $"``{methodDefinition.GenericParameters.Count}" : string.Empty)}({string.Join(", ", parameters)})";
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> for <paramref name="methodDefinition"/>.
        /// </summary>
        /// <param name="methodDefinition">The <see cref="MethodDefinition"/> to find the <see cref="XElement"/> for.</param>
        /// <param name="xDocument">The document to find <paramref name="methodDefinition"/> in.</param>
        /// <returns>The document <see cref="XElement"/> which represents <paramref name="methodDefinition"/>.</returns>
        public static XElement ToXelement(this MethodDefinition methodDefinition, XDocument xDocument)
        {
            if (methodDefinition == null || xDocument == null)
            {
                return null;
            }

            var memberNodes = from d in xDocument.Descendants()
                              where d.Name == "member"
                              select d;

            return memberNodes.SingleOrDefault(m => m.Attribute("name")?.Value == $"M:{methodDefinition.ToMethodString()}");
        }
    }
}
