// <copyright file="IContainerDocumentation.cs" company="Chris Crutchfield">
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

namespace DotNetDocs.Mixins.Contracts
{
    /// <summary>
    /// Represents a documentation container such as an Assembly, Namespace, or Type.
    /// </summary>
    public interface IContainerDocumentation : IDocumentation
    {
        /// <summary>
        /// Gets a list of the children documentations.
        /// </summary>
        IEnumerable<IDocumentation> Children { get; }

        /// <summary>
        /// Gets the object representing the object referenced by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The object to get.</param>
        /// <returns>The object representing the object referenced by <paramref name="fullName"/>.</returns>
        IDocumentation this[string fullName] { get; }

        /// <summary>
        /// Gets a value indicating if the object referenced by <paramref name="fullName"/> is contained within this container.
        /// </summary>
        /// <param name="fullName">The object to check for.</param>
        /// <returns>A value indicating if the object referenced by <paramref name="fullName"/> is contained within this container.</returns>
        bool Contains(string fullName);
    }
}
