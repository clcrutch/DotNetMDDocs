// <copyright file="TypeReferenceExtensions.cs" company="Chris Crutchfield">
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
using Mono.Cecil;

namespace DotNetMDDocs.XmlDocParser.Extensions
{
    public static class TypeReferenceExtensions
    {
        public static string DisplayName(this TypeReference typeReference)
        {
            var genericInstanceType = typeReference as GenericInstanceType;

            if (genericInstanceType?.HasGenericArguments ?? false)
            {
                var typeParams = (from t in genericInstanceType.GenericArguments
                                  select t.DisplayName()).ToArray();

                return $"{typeReference.Name.Substring(0, typeReference.Name.IndexOf('`'))}<{string.Join(", ", typeParams)}>";
            }
            else
            {
                return typeReference.Name;
            }
        }
    }
}
