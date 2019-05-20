// <copyright file="MemberDocumentationGroupGenerator.cs" company="Chris Crutchfield">
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetDocs.Mixins.Contracts;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal abstract class MemberDocumentationGroupGenerator<TDocumentation, TGenerator> : DocumentationGroupGenerator<TDocumentation, TGenerator>
        where TDocumentation : IMemberDocumentation
        where TGenerator : MemberDocumentationGenerator<TDocumentation>
    {
        public MemberDocumentationGroupGenerator(IEnumerable<TDocumentation> documentations, DirectoryInfo rootDirectory, TypeDocumentationGenerator typeDocumentationGenerator)
            : base(documentations, rootDirectory)
        {
            this.TypeDocumentationGenerator = typeDocumentationGenerator;
        }

        public FileInfo FileInfo =>
            new FileInfo(Path.Combine(this.TypeDocumentationGenerator.FileInfo.Directory.FullName, this.Documentations.First().DeclaringType.Name, $"{this.DocumentationsType}.md"));

        public TypeDocumentationGenerator TypeDocumentationGenerator { get; private set; }

        protected abstract string DocumentationsType { get; }

        public override async Task GenerateAsync()
        {
            if (!(this.Documentations?.Any() ?? false))
            {
                return;
            }

            this.FileInfo.Directory.Create();

            using (var writer = this.FileInfo.CreateText())
            {
                await writer.WriteAsync(this.GenerateDocument().Generate());
            }

            await Task.WhenAll(from d in this.Documentations
                               select ((TGenerator)Activator.CreateInstance(typeof(TGenerator), d, this.RootDirectory, this.TypeDocumentationGenerator)).GenerateAsync());
        }

        public override MDGroup GenerateTable<THeader>()
        {
            var group = new MDGroup();

            if (!(this.Documentations?.Any() ?? false))
            {
                return null;
            }

            var header = Activator.CreateInstance<THeader>();
            header.Text = this.DocumentationsType;
            group.AddElement(header);

            var table = new MDTable();
            table.Header.Cells.Add(new MDText
            {
                Text = "Name",
            });
            table.Header.Cells.Add(new MDText
            {
                Text = "Description",
            });

            foreach (var item in this.Documentations)
            {
                var generator = (TGenerator)Activator.CreateInstance(typeof(TGenerator), item, this.RootDirectory, this.TypeDocumentationGenerator);

                var row = new MDTableRow();
                row.Cells.Add(new MDLink
                {
                    Text = item.Name,
                    Url = generator.RepoUrl,
                });
                row.Cells.Add(item.Summary?.ConvertToMDGroup());
                table.Rows.Add(row);
            }

            group.AddElement(table);

            return group;
        }

        protected virtual MDDocument GenerateDocument()
        {
            var document = new MDDocument();

            document.AddElement(this.GenerateTable<MDH1>());

            return document;
        }
    }
}
