// <copyright file="ObjectDocumentation.cs" company="Chris Crutchfield">
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
using System.Xml.Linq;
using DotNetDocs.CommentBlockElements;
using DotNetDocs.Mixins;
using DotNetDocs.Mixins.Contracts;
using Mono.Cecil;

namespace DotNetDocs.ObjectDocumentations
{
    /// <summary>
    /// Represents a object documentation.  This includes types and members.
    /// </summary>
    public class ObjectDocumentation : Documentation, IObjectDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDocumentation"/> class.
        /// </summary>
        /// <param name="memberDefinition">The member to document.</param>
        /// <param name="xElement">The XML element to use for documentation.</param>
        /// <param name="declaringType">The <see cref="TypeDocumentation"/> which contains the current member.</param>
        /// <param name="declarationProvider">The declaration provider used to generate the declarationg for the current object.</param>
        protected ObjectDocumentation(IMemberDefinition memberDefinition, XElement xElement, TypeDocumentation declaringType, IDeclarationProvider declarationProvider)
            : this(new ObjectDocumentationMixin(memberDefinition, xElement, declaringType, declarationProvider))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDocumentation"/> class.
        /// </summary>
        /// <param name="mixin">The instance of <see cref="ObjectDocumentationMixin"/> used for backing this object.</param>
        protected ObjectDocumentation(ObjectDocumentationMixin mixin)
        {
            this.ObjectDocumentationMixin = mixin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDocumentation"/> class.
        /// </summary>
        protected ObjectDocumentation()
        {
        }

        /// <summary>
        /// Gets the declaration for the current object.
        /// </summary>
        public virtual string Declaration => this.ObjectDocumentationMixin.Declaration;

        /// <summary>
        /// Gets the <see cref="TypeDocumentation"/> which contains the current <see cref="ObjectDocumentation"/>.
        /// </summary>
        public TypeDocumentation DeclaringType => this.ObjectDocumentationMixin.DeclaringType;

        /// <inheritdoc />
        public override string FullName => this.ObjectDocumentationMixin.FullName;

        /// <inheritdoc />
        public override string Name => this.ObjectDocumentationMixin.Name;

        /// <summary>
        /// Gets objects representing the remarks comment.
        /// </summary>
        public virtual IEnumerable<ICommentBlockElement> Remarks => this.ObjectDocumentationMixin.Remarks;

        /// <summary>
        /// Gets objects representing the summary comment.
        /// </summary>
        public virtual IEnumerable<ICommentBlockElement> Summary => this.ObjectDocumentationMixin.Summary;

        /// <summary>
        /// Gets or sets the <see cref="ObjectDocumentationMixin"/> that backs this type.
        /// </summary>
        protected ObjectDocumentationMixin ObjectDocumentationMixin { get; set; }
    }
}
