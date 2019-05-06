﻿// <copyright file="DocumentationBase.cs" company="Chris Crutchfield">
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
            protected set => this.declaration =
                value.TrimAndCombine(Environment.NewLine)
                .Replace("[System.Runtime.CompilerServices.CompilerGenerated]", string.Empty);
        }

        /// <summary>
        /// Gets the full name of the current member.
        /// </summary>
        public virtual string FullName => this.MemberDefinition?.FullName;

        /// <summary>
        /// Gets the name for the current member.
        /// </summary>
        public virtual string Name => this.MemberDefinition?.Name;

        /// <summary>
        /// Gets the remarks for the current member.
        /// </summary>
        public virtual XmlElement Remarks => this.XElement?.Descendants().FirstOrDefault(x => x.Name == "remarks").ToXmlElement();

        /// <summary>
        /// Gets the summary from the XmlElement.
        /// </summary>
        public virtual IEnumerable<ICommentBlockElement> Summary
        {
            get
            {
                var @return = new List<ICommentBlockElement>();

                if (this.SummaryElement?.ChildNodes == null)
                {
                    return null;
                }

                foreach (var node in this.SummaryElement.ChildNodes)
                {
                    if (node is XmlText)
                    {
                        var xmlText = (XmlText)node;

                        @return.Add(new StringCommentBlockElement
                        {
                            Content = xmlText.InnerText,
                        });
                    }
                    else if (node is XmlElement)
                    {
                        var xmlElement = (XmlElement)node;

                        switch (xmlElement.Name)
                        {
                            case "see":
                                @return.Add(new SeeCommentBlockElement
                                {
                                    TypeName = xmlElement.GetAttribute("cref").Substring(2),
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

        /// <summary>
        /// Gets the summary for the current member.
        /// </summary>
        public virtual XmlElement SummaryElement
        {
            get
            {
                if (this.IsInheritedDoc)
                {
                    return this.FindBaseDocumentation()?.SummaryElement;
                }

                return this.XElement?.Descendants().FirstOrDefault(x => x.Name == "summary").ToXmlElement();
            }
        }

        /// <summary>
        /// Gets the Type which declares this field.
        /// </summary>
        protected TypeDocumentation DeclaringType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the documentation is a &#60;inheritdoc /&#62; node.
        /// </summary>
        protected bool IsInheritedDoc
        {
            get
            {
                var firstNode = this.XElement?.FirstNode;

                if (firstNode == null)
                {
                    return false;
                }

                var firstElement = firstNode as XElement;

                if (firstElement == null)
                {
                    return false;
                }

                return firstElement.Name == "inheritdoc";
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="IMemberDefinition"/> for the current member.
        /// </summary>
        protected IMemberDefinition MemberDefinition { get; private set; }

        /// <summary>
        /// Gets the XML element that represents the XML comments for the current member.
        /// </summary>
        protected XElement XElement { get; private set; }

        private DocumentationBase FindBaseDocumentation()
        {
            var baseType = this.DeclaringType.BaseType;

            if (this is MethodDocumentation)
            {
                var methodDocumentation = this as MethodDocumentation;

                var paramTypeNames = methodDocumentation.ParameterDocumentations.Select(x => x.TypeName).ToArray();
                if (methodDocumentation.IsConstructor)
                {
                    return (from m in baseType.ConstructorDocumentations
                            where m.Name == methodDocumentation.Name &&
                             !m.ParameterDocumentations.Select(x => x.TypeName).Except(paramTypeNames).Any()
                            select m).Single();
                }
                else
                {
                    return (from m in baseType.MethodDocumentations
                            where m.Name == methodDocumentation.Name &&
                             !m.ParameterDocumentations.Select(x => x.TypeName).Except(paramTypeNames).Any()
                            select m).Single();
                }
            }
            else if (this is PropertyDocumentation)
            {
                var propertyDocumentation = this as PropertyDocumentation;

                return (from p in baseType.PropertyDocumentations
                        where p.Name == propertyDocumentation.Name
                        select p).Single();
            }
            else if (this is FieldDocumentation)
            {
                var fieldDocumentation = this as FieldDocumentation;

                return (from f in baseType.FieldDocumentations
                        where f.Name == fieldDocumentation.Name
                        select f).Single();
            }

            throw new NotImplementedException();
        }
    }
}
