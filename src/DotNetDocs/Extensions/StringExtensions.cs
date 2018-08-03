// <copyright file="StringExtensions.cs" company="Chris Crutchfield">
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

using System;
using System.Linq;

namespace DotNetDocs.Extensions
{
    /// <summary>
    /// Collection of extension methods to String.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Takes an input, splits it on <see cref="Environment.NewLine"/> or '\n', trims whitespace, then combines again using the <paramref name="separator"/>.
        /// </summary>
        /// <param name="input">The input to split and combine.</param>
        /// <param name="separator">The separator used to perform the combine.</param>
        /// <returns>Null if <paramref name="input"/> is null else, the combined string.</returns>
        public static string TrimAndCombine(this string input, string separator)
        {
            if (input == null)
            {
                return null;
            }

            var split = input.Replace(Environment.NewLine, "\n").Split('\n');
            var trimmed = from s in split
                          where !string.IsNullOrWhiteSpace(s)
                          select s.Trim();

            return string.Join(separator, trimmed);
        }
    }
}
