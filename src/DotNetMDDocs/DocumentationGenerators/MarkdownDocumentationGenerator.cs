// <copyright file="MarkdownDocumentationGenerator.cs" company="Chris Crutchfield">
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
using System.Threading.Tasks;
using System.Web;
using DotNetDocs.Mixins.Contracts;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal abstract class MarkdownDocumentationGenerator<T> : DocumentationGenerator<T>
        where T : IDocumentation
    {
        public MarkdownDocumentationGenerator(T documentation, DirectoryInfo rootDirectory)
            : base(documentation, rootDirectory)
        {
        }

        public virtual string DocumentationName => this.Documentation.Name;

        public abstract string DocumentationType { get; }

        public abstract FileInfo FileInfo { get; }

        public string RepoUrl
        {
            get
            {
                var path = this.FileInfo.Directory.FullName.Replace(this.RootDirectory.Parent.FullName, string.Empty)
                                .Replace('\\', '/');
                return $"{path}/{HttpUtility.UrlEncode(this.Documentation.GetSafeName()).Replace("+", "%20")}.md";
            }
        }

        public override async Task GenerateAsync()
        {
            this.FileInfo.Directory.Create();

            using (var writer = this.FileInfo.CreateText())
            {
                await writer.WriteAsync((await this.GenerateDocumentAsync())?.Generate());
            }
        }

        protected abstract Task<MDDocument> GenerateDocumentAsync();

        protected virtual MDGroup GenerateDocumentFooter() => new MDGroup();

        protected virtual MDGroup GenerateDocumentHeader()
        {
            var group = new MDGroup();

            group.AddElement(new MDH1
            {
                Text = $"{this.DocumentationName} {this.DocumentationType}",
            });

            return group;
        }
    }
}
