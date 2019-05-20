// <copyright file="IEnumerableExtensions.cs" company="Chris Crutchfield">
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

namespace DotNetDocs.Extensions
{
    /// <summary>
    /// Extensions to <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Almost exactly the same as Linq's <see cref="Enumerable.Concat{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>, except <paramref name="second"/> cannot be null.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="first">The first <see cref="IEnumerable{T}"/> to return.  This cannot be null.</param>
        /// <param name="second">The second <see cref="IEnumerable{T}"/> to return.  This can be null.</param>
        /// <returns>If <paramref name="second"/> is null, then just <paramref name="first"/>, else <paramref name="first"/> + <paramref name="second"/>.</returns>
        public static IEnumerable<T> ConcatNull<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (second == null)
            {
                return first;
            }
            else
            {
                return first.Concat(second);
            }
        }
    }
}
