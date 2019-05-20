// <copyright file="TypeDefinitionExtensions.cs" company="Chris Crutchfield">
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
using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;

namespace DotNetDocs.Mono.Cecil.Extensions
{
    /// <summary>
    /// Extensions to <see cref="TypeDefinition"/>.
    /// </summary>
    public static class TypeDefinitionExtensions
    {
        /// <summary>
        /// Convert to <see cref="System.Reflection.Metadata.EntityHandle"/> to use with ICSharpCode.Decompiler.
        /// </summary>
        /// <param name="typeDefinition">The <see cref="TypeDefinition"/> to convert to an entity handle.</param>
        /// <param name="peFile">The <see cref="PEFile"/> which to find <paramref name="typeDefinition"/> in.</param>
        /// <returns>A reference to the entity handle for <paramref name="typeDefinition"/>.</returns>
        public static System.Reflection.Metadata.TypeDefinitionHandle? ToEntityHandle(this TypeDefinition typeDefinition, PEFile peFile)
        {
            if (peFile == null)
            {
                return null;
            }

            var typeDefs = from t in peFile.Metadata.TypeDefinitions
                           select new
                           {
                               Handle = t,
                               TypeDefinition = peFile.Metadata.GetTypeDefinition(t),
                           };

            return (from t in typeDefs
                    where peFile.Metadata.GetString(t.TypeDefinition.Namespace) == typeDefinition.Namespace &&
                       peFile.Metadata.GetString(t.TypeDefinition.Name) == typeDefinition.Name
                    select t.Handle).SingleOrDefault();
        }

        /// <summary>
        /// Converts <paramref name="typeDefinition"/> to a string compatible with XML documentations.
        /// </summary>
        /// <param name="typeDefinition">The <see cref="TypeDefinition"/> to convert.</param>
        /// <returns>A documentation compatible type string.</returns>
        public static string ToTypeString(this TypeDefinition typeDefinition)
        {
            var name = typeDefinition.Name;
            if (typeDefinition.HasGenericParameters)
            {
                name = name.Substring(0, name.Length - 2);
            }

            return $"{typeDefinition.Namespace}.{name}{(typeDefinition.HasGenericParameters ? $"{{``{typeDefinition.GenericParameters.Count - 1}}}" : string.Empty)}";
        }
    }
}
