// <copyright file="MethodDefinitionHandleExtensions.cs" company="Chris Crutchfield">
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
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;

namespace DotNetDocs.Reflection.Metadata.Extensions
{
    /// <summary>
    /// Extensions to <see cref="MethodDefinitionHandle"/>.
    /// </summary>
    public static class MethodDefinitionHandleExtensions
    {
        private static readonly Regex GenericTypeParameterRegex = new Regex(@"<(.+)>", RegexOptions.Compiled);
        private static readonly Regex ParameterRegex = new Regex(@"(?<=([\(,]))(.+?)(?=\s)", RegexOptions.Compiled);
        private static readonly Dictionary<string, string> TypeMap = new Dictionary<string, string>
        {
            { "bool", "System.Boolean" },
            { "byte", "System.Byte" },
            { "sbyte", "System.SByte" },
            { "char", "System.Char" },
            { "decimal", "System.Decimal" },
            { "double", "System.Double" },
            { "floats", "System.Single" },
            { "int", "System.Int32" },
            { "uint", "System.UInt32" },
            { "long", "System.Int64" },
            { "ulong", "System.UInt64" },
            { "object", "System.Object" },
            { "short", "System.Int16" },
            { "ushort", "System.UInt16" },
            { "string", "System.String" },
        };

        /// <summary>
        /// Gets a string representing the <see cref="MethodDefinitionHandle"/>.
        /// </summary>
        /// <param name="methodDefinitionHandle">The <see cref="MethodDefinitionHandle"/> to represent as a string.</param>
        /// <param name="peFile">The <see cref="PEFile"/> which to find <paramref name="methodDefinitionHandle"/> in.</param>
        /// <param name="decompiler">An instance of <see cref="CSharpDecompiler"/> used for finding method parameters.</param>
        /// <returns>A string repsentation of <paramref name="methodDefinitionHandle"/>.</returns>
        public static string ToMethodString(this MethodDefinitionHandle methodDefinitionHandle, PEFile peFile, CSharpDecompiler decompiler)
        {
            var methodDefinition = peFile.Metadata.GetMethodDefinition(methodDefinitionHandle);
            var methodDeclaration = decompiler.DecompileAsString(methodDefinitionHandle).Replace("(this ", "(");

            var parameters = from m in ParameterRegex.Matches(methodDeclaration).Cast<Match>()
                             select ParseTypeString(m.Value.Trim());

            var fullParameters = from p in parameters
                                 where p != ");"
                                 select TypeMap.ContainsKey(p) ? TypeMap[p] : p;

            var declaringType = peFile.Metadata.GetTypeDefinition(methodDefinition.GetDeclaringType());
            var @namespace = peFile.Metadata.GetString(declaringType.Namespace);
            var typeName = peFile.Metadata.GetString(declaringType.Name);
            var methodName = peFile.Metadata.GetString(methodDefinition.Name);

            bool isConstructor = methodName == ".ctor";

            return $"{@namespace}.{typeName}.{(isConstructor ? "#ctor" : methodName)}{(methodDefinition.GetGenericParameters().Any() ? $"``{methodDefinition.GetGenericParameters().Count}" : string.Empty)}({string.Join(", ", fullParameters)})";
        }

        private static string ParseTypeString(string typeString)
        {
            var match = GenericTypeParameterRegex.Match(typeString);
            if (!match.Success)
            {
                if (typeString.EndsWith("?"))
                {
                    return $"System.Nullable{{{typeString.Substring(0, typeString.Length - 1)}}}";
                }
                else
                {
                    return typeString;
                }
            }

            var genericTypeParameters = from g in match.Value.Split(',')
                                        where !string.IsNullOrWhiteSpace(g)
                                        select g;

            return $"{typeString.Replace(match.Value, string.Empty)}{{``{genericTypeParameters.Count() - 1}}}";
        }
    }
}
