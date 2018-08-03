// <copyright file="DocumentationBase.cs" company="Chris Crutchfield">
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
using System.Xml;
using System.Xml.Linq;
using DotNetDocs.Extensions;
using Mono.Cecil;

namespace DotNetDocs
{
    /// <summary>
    /// Parses a generic <see cref="IMemberDefinition"/> and XML element.
    /// </summary>
    public abstract class DocumentationBase
    {
        private string declaration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationBase"/> class.
        /// </summary>
        /// <param name="memberDefinition">The <see cref="IMemberDefinition"/> which to document.</param>
        /// <param name="xElement">The XML element representing the XML comments for the current member.</param>
        /// <param name="declaringType">The type which declares this member.</param>
        public DocumentationBase(IMemberDefinition memberDefinition, XElement xElement, TypeDocumentation declaringType)
        {
            this.MemberDefinition = memberDefinition;
            this.XElement = xElement;
            this.DeclaringType = declaringType;
        }

        /// <summary>
        /// Gets or sets the code declaration for the current member.
        /// </summary>
        public virtual string Declaration
        {
            get => this.declaration;
            protected set => this.declaration = value.TrimAndCombine(Environment.NewLine);
        }

        /// <summary>
        /// Gets the full name of the current member.
        /// </summary>
        public virtual string FullName => MemberDefinition?.FullName;

        /// <summary>
        /// Gets the name for the current member.
        /// </summary>
        public virtual string Name => MemberDefinition?.Name;

        /// <summary>
        /// Gets the remarks for the current member.
        /// </summary>
        public virtual XmlElement Remarks => XElement?.Descendants().FirstOrDefault(x => x.Name == "remarks").ToXmlElement();

        /// <summary>
        /// Gets the summary for the current member.
        /// </summary>
        public virtual XmlElement Summary => XElement?.Descendants().FirstOrDefault(x => x.Name == "summary").ToXmlElement();

        /// <summary>
        /// Gets the Type which declares this field.
        /// </summary>
        protected TypeDocumentation DeclaringType { get; private set; }

        /// <summary>
        /// Gets the underlying <see cref="IMemberDefinition"/> for the current member.
        /// </summary>
        protected IMemberDefinition MemberDefinition { get; private set; }

        /// <summary>
        /// Gets the XML element that represents the XML comments for the current member.
        /// </summary>
        protected XElement XElement { get; private set; }
    }
}
