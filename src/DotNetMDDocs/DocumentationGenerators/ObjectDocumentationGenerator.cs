// <copyright file="ObjectDocumentationGenerator.cs" company="Chris Crutchfield">
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
using System.IO;
using DotNetDocs.ContainerDocumentations;
using DotNetDocs.Mixins.Contracts;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal abstract class ObjectDocumentationGenerator<T> : MarkdownDocumentationGenerator<T>
        where T : IObjectDocumentation
    {
        public ObjectDocumentationGenerator(T documentation, DirectoryInfo rootDirectory)
            : base(documentation, rootDirectory)
        {
        }

        public AssemblyDocumentation AssemblyDocumentation => this.Documentation.DeclaringType.DeclaringAssembly;

        protected override MDGroup GenerateDocumentFooter()
        {
            var group = base.GenerateDocumentFooter();

            var remarksGroup = this.Documentation.Remarks?.ConvertToMDGroup();
            if (remarksGroup != null)
            {
                group.AddElement(new MDH2
                {
                    Text = "Remarks",
                });
                group.AddElement(remarksGroup);
            }

            return group;
        }

        protected override MDGroup GenerateDocumentHeader()
        {
            var group = base.GenerateDocumentHeader();

            var summaryGroup = this.Documentation.Summary?.ConvertToMDGroup();
            if (summaryGroup != null)
            {
                group.AddElement(new MDQuote
                {
                    Quote = summaryGroup,
                });
            }

            group.AddElement(new MDBold
            {
                Text = "Namespace:",
            });

            group.AddElement(new MDText
            {
                Text = $" {this.Documentation.DeclaringType.NamespaceDocumentation.FullName}{Environment.NewLine}{Environment.NewLine}",
            });

            group.AddElement(new MDBold
            {
                Text = "Assembly:",
            });

            group.AddElement(new MDText
            {
                Text = $" {this.AssemblyDocumentation.Name} (in {this.AssemblyDocumentation.FileName}){Environment.NewLine}",
            });

            return group;
        }

        protected virtual MDGroup GenerateSyntax()
        {
            var group = new MDGroup();

            group.AddElement(new MDH2
            {
                Text = "Syntax",
            });

            group.AddElement(new MDCode
            {
                Code = this.Documentation.Declaration,
                Language = "csharp",
            });

            return group;
        }
    }
}
