// <copyright file="ObjectDocumentationMixin.cs" company="Chris Crutchfield">
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
using System.Xml;
using System.Xml.Linq;
using DotNetDocs.CommentBlockElements;
using DotNetDocs.Extensions;
using DotNetDocs.Mixins.Contracts;
using DotNetDocs.ObjectDocumentations;
using Mono.Cecil;

namespace DotNetDocs.Mixins
{
    /// <summary>
    /// Represents a mixin for objects such as types and members.
    /// </summary>
    public class ObjectDocumentationMixin : IObjectDocumentation
    {
        private readonly IDeclarationProvider declarationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDocumentationMixin"/> class.
        /// </summary>
        /// <param name="memberDefinition">The member to document.</param>
        /// <param name="xElement">The XML element to use for documentation.</param>
        /// <param name="declaringType">The <see cref="TypeDocumentation"/> which contains the current object.</param>
        /// <param name="declarationProvider">The provider to use for generating the declaration for the object.</param>
        public ObjectDocumentationMixin(IMemberDefinition memberDefinition, XElement xElement, TypeDocumentation declaringType, IDeclarationProvider declarationProvider)
        {
            this.MemberDefinition = memberDefinition;
            this.XElement = xElement;
            this.DeclaringType = declaringType;
            this.declarationProvider = declarationProvider;
        }

        /// <summary>
        /// Gets the declaration for the member.
        /// </summary>
        public string Declaration => this.declarationProvider?.GetDeclaration();

        /// <summary>
        /// Gets the <see cref="TypeDocumentation"/> which contains the current documentation.
        /// </summary>
        public TypeDocumentation DeclaringType { get; private set; }

        /// <summary>
        /// Gets the full name for the member.
        /// </summary>
        public string FullName => this.MemberDefinition?.FullName;

        /// <summary>
        /// Gets the name for the member.
        /// </summary>
        public string Name => this.MemberDefinition?.Name;

        /// <summary>
        /// Gets objects representing the remarks comments.
        /// </summary>
        public virtual IEnumerable<ICommentBlockElement> Remarks =>
            this.ParseCommentXmlElement(this.RemarksElement);

        /// <summary>
        /// Gets objects representing the summary comment.
        /// </summary>
        public virtual IEnumerable<ICommentBlockElement> Summary =>
            this.ParseCommentXmlElement(this.SummaryElement);

        /// <summary>
        /// Gets the Mono.Cecil member definition.
        /// </summary>
        protected internal IMemberDefinition MemberDefinition { get; private set; }

        /// <summary>
        /// Gets the XML element for the object.
        /// </summary>
        protected XElement XElement { get; private set; }

        private XmlElement RemarksElement =>
            this.XElement?.Descendants()?.FirstOrDefault(x => x.Name == "remarks").ToXmlElement();

        private XmlElement SummaryElement =>
            this.XElement?.Descendants()?.FirstOrDefault(x => x.Name == "summary").ToXmlElement();

        private IEnumerable<ICommentBlockElement> ParseCommentXmlElement(XmlElement element)
        {
            var @return = new List<ICommentBlockElement>();

            if (element?.ChildNodes == null)
            {
                return null;
            }

            foreach (var node in element.ChildNodes)
            {
                if (node is XmlText xmlText)
                {
                    @return.Add(new StringCommentBlockElement
                    {
                        Content = xmlText.InnerText,
                    });
                }
                else if (node is XmlElement xmlElement)
                {
                    switch (xmlElement.Name)
                    {
                        case "see":
                            @return.Add(new SeeCommentBlockElement
                            {
                                TypeName = xmlElement.GetAttribute("cref").Substring(2),
                            });
                            break;
                        case "paramref":
                            @return.Add(new ParamRefCommentBlockElement
                            {
                                ParameterName = xmlElement.GetAttribute("name"),
                            });
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return @return;
        }
    }
}
