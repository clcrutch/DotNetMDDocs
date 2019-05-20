// <copyright file="TypeDocumentationGenerator.cs" company="Chris Crutchfield">
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
using System.Text;
using System.Threading.Tasks;
using DotNetDocs.MemberDocumentations;
using DotNetDocs.Mixins.Contracts;
using DotNetDocs.ObjectDocumentations;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal class TypeDocumentationGenerator : ObjectDocumentationGenerator<TypeDocumentation>
    {
        private int currInheritanceLevel = 1;

        public TypeDocumentationGenerator(TypeDocumentation documentation, DirectoryInfo rootDirectory)
            : base(documentation, rootDirectory)
        {
            UrlHelper.AddType(documentation.FullName, this.RepoUrl);
        }

        private enum MemberType
        {
            Unknown,
            Field,
            Property,
            Constructor,
            Method,
        }

        public override string DocumentationType
        {
            get
            {
                if (this.Documentation.IsInterface)
                {
                    return "Interface";
                }
                else if (this.Documentation.IsClass)
                {
                    return "Class";
                }
                else if (this.Documentation.IsEnum)
                {
                    return "Enum";
                }

                return "Unknown";
            }
        }

        public override FileInfo FileInfo =>
            new FileInfo($"{Path.Combine(this.RootDirectory.FullName, Path.Combine(this.Documentation.NamespaceDocumentation.FullName.Split('.')), this.Documentation.GetSafeName())}.md");

        protected override async Task<MDDocument> GenerateDocumentAsync()
        {
            var document = new MDDocument();

            document.AddElement(this.GenerateDocumentHeader());

            document.AddElement(new MDH2
            {
                Text = "Inheritance Hierarchy",
            });

            document.AddElement(this.GenerateInheritanceLevel(this.Documentation));

            document.AddElement(this.GenerateSyntax());

            document.AddElementRange(
                await Task.WhenAll(
                    this.GenerateMemberDocumentationGroupAsync<MethodDocumentation, MethodDocumentationGenerator, MethodDocumentationGroupGenerator>(this.Documentation.ConstructorDocumentations),
                    this.GenerateMemberDocumentationGroupAsync<PropertyDocumentation, PropertyDocumentationGenerator, PropertyDocumentationGroupGenerator>(this.Documentation.PropertyDocumentations),
                    this.GenerateMemberDocumentationGroupAsync<MethodDocumentation, MethodDocumentationGenerator, MethodDocumentationGroupGenerator>(this.Documentation.MethodDocumentations),
                    this.GenerateMemberDocumentationGroupAsync<FieldDocumentation, FieldDocumentationGenerator, FieldDocumentationGroupGenerator>(this.Documentation.FieldDocumentations)));

            return document;
        }

        private MDGroup GenerateInheritanceLevel(TypeDocumentation typeDocumentation)
        {
            var group = new MDGroup();

            if (typeDocumentation.BaseType != null)
            {
                group.AddElement(this.GenerateInheritanceLevel(typeDocumentation.BaseType));
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < this.currInheritanceLevel; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    stringBuilder.Append(SPACE);
                }
            }

            this.currInheritanceLevel++;

            group.AddElement(new MDText
            {
                Text = stringBuilder.ToString(),
            });
            group.AddElement(new MDLink
            {
                Url = UrlHelper.GetUrl(typeDocumentation.FullName),
                Text = typeDocumentation.FullName,
            });
            group.AddElement(new MDText
            {
                Text = $"{Environment.NewLine}{Environment.NewLine}",
            });

            return group;
        }

        private async Task<MDGroup> GenerateMemberDocumentationGroupAsync<TDocumentation, TGenerator, TGroupGenerator>(IEnumerable<TDocumentation> documentations)
            where TDocumentation : IMemberDocumentation
            where TGenerator : MemberDocumentationGenerator<TDocumentation>
            where TGroupGenerator : MemberDocumentationGroupGenerator<TDocumentation, TGenerator>
        {
            var documentationGroupGenerator = (TGroupGenerator)Activator.CreateInstance(typeof(TGroupGenerator), documentations, this.RootDirectory, this);

            await documentationGroupGenerator.GenerateAsync();

            return documentationGroupGenerator.GenerateTable<MDH2>();
        }
    }
}
