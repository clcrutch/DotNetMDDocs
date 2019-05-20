// <copyright file="MethodInputOutputDocumentationBase.cs" company="Chris Crutchfield">
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

using System.Xml.Linq;

namespace DotNetDocs.MemberDocumentations
{
    /// <summary>
    /// The base class for parameters and return values for a method.
    /// </summary>
    public abstract class MethodInputOutputDocumentationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInputOutputDocumentationBase"/> class.
        /// </summary>
        /// <param name="xElement">The XML element representing the XML comments for the current member.</param>
        protected internal MethodInputOutputDocumentationBase(XElement xElement)
        {
            this.XElement = xElement;
        }

        /// <summary>
        /// Gets the summary for the current member.
        /// </summary>
        public virtual string Summary => this.XElement?.Value;

        /// <summary>
        /// Gets the type name for the current member.
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// Gets the XML element that represents the XML comments for the current member.
        /// </summary>
        protected XElement XElement { get; private set; }
    }
}
