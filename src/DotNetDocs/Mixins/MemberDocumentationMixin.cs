// <copyright file="MemberDocumentationMixin.cs" company="Chris Crutchfield">
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DotNetDocs.CommentBlockElements;
using DotNetDocs.Mixins.Contracts;
using DotNetDocs.Mono.Cecil.Extensions;
using DotNetDocs.ObjectDocumentations;
using Mono.Cecil;

namespace DotNetDocs.Mixins
{
    /// <summary>
    /// Represents a mixin for members.
    /// </summary>
    public sealed class MemberDocumentationMixin : ObjectDocumentationMixin, IMemberDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDocumentationMixin"/> class.
        /// </summary>
        /// <param name="memberDefinition">The member to document.</param>
        /// <param name="xElement">The XML element to use for documentation.</param>
        /// <param name="declaringType">The <see cref="TypeDocumentation"/> which contains the current member.</param>
        /// <param name="declarationProvider">The provider to use for generating the declaration for the member.</param>
        public MemberDocumentationMixin(IMemberDefinition memberDefinition, XElement xElement, TypeDocumentation declaringType, IDeclarationProvider declarationProvider)
            : base(memberDefinition, xElement, declaringType, declarationProvider)
        {
        }

        /// <summary>
        /// Gets objects representing the remarks comment.  Handles inheritdoc elements as well.
        /// </summary>
        public override IEnumerable<ICommentBlockElement> Remarks
        {
            get
            {
                if (this.IsInheritedDoc)
                {
                    return this.FindBaseDocumentation()?.Remarks;
                }

                return base.Remarks;
            }
        }

        /// <summary>
        /// Gets objects representing the summary comment.  Handles inheritdoc elements as well.
        /// </summary>
        public override IEnumerable<ICommentBlockElement> Summary
        {
            get
            {
                if (this.IsInheritedDoc)
                {
                    return this.FindBaseDocumentation()?.Summary;
                }

                return base.Summary;
            }
        }

        private bool IsInheritedDoc
        {
            get
            {
                var firstNode = this.XElement?.FirstNode;

                if (firstNode == null)
                {
                    return false;
                }

                if (!(firstNode is XElement firstElement))
                {
                    return false;
                }

                return firstElement.Name == "inheritdoc";
            }
        }

        private IObjectDocumentation FindBaseDocumentation()
        {
            var baseType = this.DeclaringType.BaseType;
            if (baseType == null)
            {
                return null;
            }

            if (this.MemberDefinition is MethodDefinition)
            {
                var methodDefinition = this.MemberDefinition as MethodDefinition;

                var methodUniqueString = methodDefinition.ToMethodString();
                if (methodDefinition.IsConstructor)
                {
                    return (from m in baseType.ConstructorDocumentations
                            where m.ToMethodString() == methodDefinition.ToMethodString()
                            select m).SingleOrDefault();
                }
                else
                {
                    return (from m in baseType.MethodDocumentations
                            where m.ToMethodString() == methodDefinition.ToMethodString()
                            select m).SingleOrDefault();
                }
            }
            else if (this.MemberDefinition is PropertyDefinition)
            {
                var propertyDefinition = this.MemberDefinition as PropertyDefinition;

                return (from p in baseType.PropertyDocumentations
                        where p.Name == propertyDefinition.Name
                        select p).SingleOrDefault();
            }
            else if (this.MemberDefinition is FieldDefinition)
            {
                var fieldDefinition = this.MemberDefinition as FieldDefinition;

                return (from f in baseType.FieldDocumentations
                        where f.Name == fieldDefinition.Name
                        select f).Single();
            }

            throw new NotImplementedException();
        }
    }
}
