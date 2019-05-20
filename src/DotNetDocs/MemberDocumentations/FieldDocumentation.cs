// <copyright file="FieldDocumentation.cs" company="Chris Crutchfield">
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

using System.Reflection.Metadata;
using System.Xml.Linq;
using DotNetDocs.Mixins;
using DotNetDocs.ObjectDocumentations;
using FieldDefinition = Mono.Cecil.FieldDefinition;

namespace DotNetDocs.MemberDocumentations
{
    /// <summary>
    /// Parses a field.
    /// </summary>
    public class FieldDocumentation : MemberDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDocumentation"/> class.
        /// </summary>
        /// <param name="fieldDefinition">The <see cref="FieldDefinition"/> which to document.</param>
        /// <param name="xElement">The XML element representing the XML comments for the current member.</param>
        /// <param name="handle">The <see cref="EntityHandle"/> that represents the member to document.</param>
        /// <param name="declaringType">The type which declares this member.</param>
        protected internal FieldDocumentation(FieldDefinition fieldDefinition, XElement xElement, EntityHandle? handle, TypeDocumentation declaringType)
            : base(fieldDefinition, xElement, declaringType, new FieldDeclarationProvider(declaringType.DeclaringAssembly.Decompiler, handle))
        {
        }
    }
}
