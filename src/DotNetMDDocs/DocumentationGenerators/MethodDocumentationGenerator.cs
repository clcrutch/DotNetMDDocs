// <copyright file="MethodDocumentationGenerator.cs" company="Chris Crutchfield">
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
using System.Linq;
using System.Threading.Tasks;
using DotNetDocs.MemberDocumentations;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal class MethodDocumentationGenerator : MemberDocumentationGenerator<MethodDocumentation>
    {
        public MethodDocumentationGenerator(MethodDocumentation documentation, DirectoryInfo rootDirectory, TypeDocumentationGenerator typeDocumentationGenerator)
            : base(documentation, rootDirectory, typeDocumentationGenerator)
        {
        }

        public override string DocumentationType
        {
            get
            {
                if (this.Documentation.IsConstructor)
                {
                    return "Constructor";
                }
                else
                {
                    return "Method";
                }
            }
        }

        protected override Task<MDDocument> GenerateDocumentAsync()
        {
            var document = new MDDocument();

            document.AddElement(this.GenerateDocumentHeader());

            if (this.Documentation.ParameterDocumentations.Any())
            {
                document.AddElement(new MDH5
                {
                    Text = "Parameters",
                });

                foreach (var param in this.Documentation.ParameterDocumentations)
                {
                    document.AddElement(new MDItalics
                    {
                        Text = param.Name,
                    });
                    document.AddElement(new MDText
                    {
                        Text = $"{Environment.NewLine}{Environment.NewLine}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}",
                    });
                    document.AddElement(new MDText
                    {
                        Text = $"Type: {param.TypeName}{Environment.NewLine}{Environment.NewLine}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}",
                    });
                    document.AddElement(new MDText
                    {
                        Text = $"{param.Summary}{Environment.NewLine}{Environment.NewLine}",
                    });
                    document.AddElement(new MDText
                    {
                        Text = $"{Environment.NewLine}",
                    });
                }
            }

            document.AddElement(new MDH5
            {
                Text = "Return Value",
            });

            document.AddElement(new MDText
            {
                Text = $"Type: {this.Documentation.ReturnValueDocumentation.TypeName}{Environment.NewLine}{Environment.NewLine}",
            });
            document.AddElement(new MDText
            {
                Text = $"{this.Documentation.ReturnValueDocumentation.Summary}{Environment.NewLine}{Environment.NewLine}",
            });

            return Task.FromResult(document);
        }
    }
}
