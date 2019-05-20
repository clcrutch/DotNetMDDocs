// <copyright file="ContainerDocumentation.cs" company="Chris Crutchfield">
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
using DotNetDocs.Mixins;
using DotNetDocs.Mixins.Contracts;

namespace DotNetDocs.ContainerDocumentations
{
    /// <summary>
    /// Represents a container documentation.  This includes assemblies, namespaces, and types.
    /// </summary>
    public abstract class ContainerDocumentation : Documentation, IContainerDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerDocumentation"/> class.
        /// </summary>
        protected ContainerDocumentation()
        {
            this.ContainerDocumentationMixin = new ContainerDocumentationMixin(this);
        }

        /// <summary>
        /// Gets the children for the current container.
        /// </summary>
        public abstract IEnumerable<IDocumentation> Children { get; }

        /// <summary>
        /// Gets the <see cref="ContainerDocumentationMixin"/> which backs the current container.
        /// </summary>
        protected ContainerDocumentationMixin ContainerDocumentationMixin { get; private set; }

        /// <summary>
        /// Gets the object represented by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The key to use to get the object from the container.</param>
        /// <returns>The object represented by the <paramref name="fullName"/>.</returns>
        public IDocumentation this[string fullName] => this.ContainerDocumentationMixin[fullName];

        /// <summary>
        /// Get a value indicating if the current container contains an object represented by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The key to use to check if the object is in the container.</param>
        /// <returns>A value indicating if the current container contains an object represented by <paramref name="fullName"/>.</returns>
        public bool Contains(string fullName) => this.ContainerDocumentationMixin.Contains(fullName);
    }
}
