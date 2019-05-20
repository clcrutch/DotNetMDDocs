// <copyright file="ContainerDocumentationMixin.cs" company="Chris Crutchfield">
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
using DotNetDocs.Mixins.Contracts;

namespace DotNetDocs.Mixins
{
    /// <summary>
    /// Represents a mixin for containers.
    /// </summary>
    public sealed class ContainerDocumentationMixin : IContainerDocumentation
    {
        private readonly IContainerDocumentation self;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerDocumentationMixin"/> class.
        /// </summary>
        /// <param name="self">A reference to an implementation of <see cref="IContainerDocumentation"/>.</param>
        public ContainerDocumentationMixin(IContainerDocumentation self)
        {
            this.self = self;
        }

        /// <summary>
        /// Gets the children of the container.
        /// </summary>
        public IEnumerable<IDocumentation> Children => this.self.Children;

        /// <summary>
        /// Gets the full name of the container.
        /// </summary>
        public string FullName => this.self.FullName;

        /// <summary>
        /// Gets the name of the container.
        /// </summary>
        public string Name => this.self.Name;

        /// <summary>
        /// Gets the documentation referenced by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The full name to use when finding the documentation.</param>
        /// <returns>The documentation referenced by <paramref name="fullName"/>.</returns>
        public IDocumentation this[string fullName]
        {
            get
            {
                // Are we the match?
                if (this.FullName == fullName)
                {
                    return this;
                }

                // Is one of our direct children a match?
                var @return = this.Children.SingleOrDefault(c => c.FullName == fullName);
                if (@return != null)
                {
                    return @return;
                }

                // Filter our children to just container documentations.
                var childContainers = from c in this.Children
                                      where c is IContainerDocumentation
                                      select (IContainerDocumentation)c;

                // Search in our container children to see if one of their descendants is a match.
                @return = (from c in childContainers
                           where c.Contains(fullName)
                           select c[fullName]).SingleOrDefault();
                if (@return != null)
                {
                    return @return;
                }

                // We found nothing.
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the container contains a documenation referenced by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The full name to use when checking for the documentation.</param>
        /// <returns>A value indicating whether the container contains a documentation referenced by <paramref name="fullName"/>.</returns>
        public bool Contains(string fullName)
        {
            // Are we the match?
            if (this.FullName == fullName)
            {
                return true;
            }

            // Is one of our direct children a match?
            if (this.Children.Any(c => c.FullName == fullName))
            {
                return true;
            }

            // Filter our children to just container documentations.
            var childContainers = from c in this.Children
                                  where c is IContainerDocumentation
                                  select (IContainerDocumentation)c;

            // Search in our container children to see if one of their descendants is a match.
            return childContainers.Any(c => c.Contains(fullName));
        }
    }
}
