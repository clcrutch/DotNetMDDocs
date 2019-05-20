// <copyright file="MemberDocumentation.cs" company="Chris Crutchfield">
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
using DotNetDocs.Mixins;
using DotNetDocs.Mixins.Contracts;
using DotNetDocs.ObjectDocumentations;
using Mono.Cecil;

namespace DotNetDocs.MemberDocumentations
{
    /// <summary>
    /// Documentation for a member.
    /// </summary>
    public abstract class MemberDocumentation : ObjectDocumentation, IMemberDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDocumentation"/> class.
        /// </summary>
        /// <param name="memberDefinition">The member to document.</param>
        /// <param name="xElement">The XML element to use for documentation.</param>
        /// <param name="declaringType">The <see cref="TypeDocumentation"/> which contains the current member.</param>
        /// <param name="declarationProvider">The declaration provider used to generate the declarationg for the current member.</param>
        protected MemberDocumentation(IMemberDefinition memberDefinition, XElement xElement, TypeDocumentation declaringType, IDeclarationProvider declarationProvider)
            : base(new MemberDocumentationMixin(memberDefinition, xElement, declaringType, declarationProvider))
        {
        }
    }
}
