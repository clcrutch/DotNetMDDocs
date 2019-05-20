// <copyright file="MemberDocumentationGenerator.cs" company="Chris Crutchfield">
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

using System.IO;
using DotNetDocs.Mixins.Contracts;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal abstract class MemberDocumentationGenerator<T> : ObjectDocumentationGenerator<T>
        where T : IMemberDocumentation
    {
        public MemberDocumentationGenerator(T documentation, DirectoryInfo rootDirectory, TypeDocumentationGenerator typeDocumentationGenerator)
            : base(documentation, rootDirectory)
        {
            this.TypeDocumentationGenerator = typeDocumentationGenerator;
        }

        public virtual string DocumentationGroupType => $"{this.DocumentationType}s";

        public override string DocumentationName => $"{this.Documentation.DeclaringType.Name}.{base.DocumentationName}";

        public override FileInfo FileInfo =>
            new FileInfo(Path.Combine(this.TypeDocumentationGenerator.FileInfo.Directory.FullName, this.Documentation.DeclaringType.Name, this.DocumentationGroupType, $"{this.Documentation.GetSafeName()}.md"));

        public TypeDocumentationGenerator TypeDocumentationGenerator { get; private set; }

        protected override MDGroup GenerateDocumentHeader()
        {
            var group = base.GenerateDocumentHeader();

            group.AddElement(this.GenerateSyntax());

            return group;
        }
    }
}
