// <copyright file="ModuleDefinitionExtensions.cs" company="Chris Crutchfield">
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
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace DotNetDocs.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ModuleDefinition"/>.
    /// </summary>
    internal static class ModuleDefinitionExtensions
    {
        /// <summary>
        /// Gets a list of all namespaces for <paramref name="moduleDefinition"/>..
        /// </summary>
        /// <param name="moduleDefinition">The module to get namespaces for.</param>
        /// <returns>A list of all namespaces for <paramref name="moduleDefinition"/>.</returns>
        public static IEnumerable<string> GetNamespaces(this ModuleDefinition moduleDefinition) =>
            (from t in moduleDefinition.Types
             where !string.IsNullOrWhiteSpace(t.Namespace) &&
                t.Namespace != "Microsoft.CodeAnalysis" &&
                t.Namespace != "System.Runtime.CompilerServices"
             select SplitNamespace(t.Namespace))
                              .SelectMany(n => n)
                              .Distinct()
                              .OrderBy(n => n)
                              .ToArray();

        /// <summary>
        /// Gets the root namespaces for <paramref name="moduleDefinition"/>.
        /// </summary>
        /// <param name="moduleDefinition">The module to get root namespaces for.</param>
        /// <returns>A list of the root namespaces for <paramref name="moduleDefinition"/>.</returns>
        public static IEnumerable<string> GetRootNamespaces(this ModuleDefinition moduleDefinition)
        {
            var namespaces = moduleDefinition.GetNamespaces();

            var namespacesToRemove = new HashSet<string>();
            foreach (var baseNamespace in namespaces)
            {
                if (namespacesToRemove.Contains(baseNamespace))
                {
                    continue;
                }

                foreach (var @namespace in namespaces)
                {
                    if (baseNamespace == @namespace ||
                        namespacesToRemove.Contains(@namespace))
                    {
                        continue;
                    }

                    if (@namespace.StartsWith(baseNamespace))
                    {
                        namespacesToRemove.Add(@namespace);
                    }
                }
            }

            return (from n in namespaces.Except(namespacesToRemove)
                    select n).ToArray();
        }

        private static IEnumerable<string> SplitNamespace(string @namespace)
        {
            var namespaceParts = @namespace.Split('.');

            for (int i = 0; i < namespaceParts.Length; i++)
            {
                var namespaceBuilder = new StringBuilder();

                for (int j = 0; j <= i; j++)
                {
                    if (namespaceBuilder.Length > 0)
                    {
                        namespaceBuilder.Append('.');
                    }

                    namespaceBuilder.Append(namespaceParts[j]);
                }

                yield return namespaceBuilder.ToString();
            }
        }
    }
}
