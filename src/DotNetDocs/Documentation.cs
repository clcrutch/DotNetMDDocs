﻿// <copyright file="Documentation.cs" company="Chris Crutchfield">
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

using DotNetDocs.Mixins.Contracts;

namespace DotNetDocs
{
    /// <summary>
    /// Represents a generic documentation.
    /// </summary>
    public abstract class Documentation : IDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Documentation"/> class.
        /// </summary>
        protected Documentation()
        {
        }

        /// <summary>
        /// Gets the full name of the current member.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Gets the name for the current member.
        /// </summary>
        public abstract string Name { get; }
    }
}
