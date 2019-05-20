// <copyright file="MethodDocumentation.cs" company="Chris Crutchfield">
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

using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using DotNetDocs.Mixins;
using DotNetDocs.Mono.Cecil.Extensions;
using DotNetDocs.ObjectDocumentations;
using MethodDefinition = Mono.Cecil.MethodDefinition;

namespace DotNetDocs.MemberDocumentations
{
    /// <summary>
    /// Parses a method.
    /// </summary>
    public class MethodDocumentation : MemberDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDocumentation"/> class.
        /// </summary>
        /// <param name="methodDefinition">The <see cref="MethodDefinition"/> which to document.</param>
        /// <param name="xElement">The XML element representing the XML comments for the current member.</param>
        /// <param name="handle">The <see cref="EntityHandle"/> that represents the member to document.</param>
        /// <param name="declaringType">The type which declares this member.</param>
        protected internal MethodDocumentation(MethodDefinition methodDefinition, XElement xElement, EntityHandle? handle, TypeDocumentation declaringType)
            : base(methodDefinition, xElement, declaringType, new SimpleDeclarationProvider(declaringType.DeclaringAssembly.Decompiler, handle))
        {
            this.ParameterDocumentations = this.GetParameterDocumentations(methodDefinition, xElement);
            this.ReturnValueDocumentation = new ReturnValueDocumentation(methodDefinition.MethodReturnType, xElement?.Descendants()?.SingleOrDefault(x => x.Name == "returns"));
        }

        /// <summary>
        /// Gets a value indicating whether the current method is a constructor.
        /// </summary>
        public bool IsConstructor => this.MethodDefinition?.IsConstructor ?? false;

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                var name = base.Name;
                if (this.IsConstructor)
                {
                    name = "Constructor";
                }

                var parameters = (from p in this.MethodDefinition.Parameters
                                  select p.ParameterType.Name).ToArray();

                return $"{name}({string.Join(", ", parameters)})";
            }
        }

        /// <summary>
        /// Gets a list of the parameters this method takes.
        /// </summary>
        public ParameterDocumentation[] ParameterDocumentations { get; private set; }

        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        public ReturnValueDocumentation ReturnValueDocumentation { get; private set; }

        /// <summary>
        /// Gets the underlying <see cref="MethodDefinition"/> for the current documentation.
        /// </summary>
        protected MethodDefinition MethodDefinition => (MethodDefinition)this.MemberDocumentationMixin.MemberDefinition;

        private MemberDocumentationMixin MemberDocumentationMixin =>
            (MemberDocumentationMixin)this.ObjectDocumentationMixin;

        /// <summary>
        /// Converts the current <see cref="MethodDocumentation"/> to a string.
        /// </summary>
        /// <returns>A string representing the current method.</returns>
        internal string ToMethodString() =>
            ((MethodDefinition)this.MemberDocumentationMixin.MemberDefinition).ToMethodString();

        private ParameterDocumentation[] GetParameterDocumentations(MethodDefinition methodDefinition, XElement xElement) =>
            (from p in methodDefinition.Parameters
             select new ParameterDocumentation(p, xElement?.Descendants()?.SingleOrDefault(x => x.Name == "param" && x.Attribute("name").Value == p.Name))).ToArray();
    }
}
