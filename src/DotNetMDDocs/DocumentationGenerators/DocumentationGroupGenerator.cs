// <copyright file="DocumentationGroupGenerator.cs" company="Chris Crutchfield">
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
using System.IO;
using DotNetDocs.Mixins.Contracts;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal abstract class DocumentationGroupGenerator<TDocumentation, TGenerator> : Generator
        where TDocumentation : IDocumentation
        where TGenerator : MarkdownDocumentationGenerator<TDocumentation>
    {
        public DocumentationGroupGenerator(IEnumerable<TDocumentation> documentations, DirectoryInfo rootDirectory)
            : base(rootDirectory)
        {
            this.Documentations = documentations;
        }

        public IEnumerable<TDocumentation> Documentations { get; set; }

        public abstract MDGroup GenerateTable<THeader>()
            where THeader : MDHeader;
    }
}
