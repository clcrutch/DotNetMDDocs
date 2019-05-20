// <copyright file="NamespaceDocumentationGenerator.cs" company="Chris Crutchfield">
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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetDocs.ContainerDocumentations;
using DotNetDocs.ObjectDocumentations;
using DotNetMDDocs.Extensions;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal class NamespaceDocumentationGenerator : MarkdownDocumentationGenerator<NamespaceDocumentation>
    {
        private static readonly Regex ExistingNameRegex = new Regex(@"(?<=|\[)([\w\.])+(?=])", RegexOptions.Compiled);
        private static readonly Regex ExistingUrlRegex = new Regex(@"(?<=\()([\w\./])+(?=\))", RegexOptions.Compiled);

#pragma warning disable IDE0044 // Add readonly modifier.  These are referenced via reflection.
        private List<NamespaceObject> existingNamespaces = new List<NamespaceObject>();
        private List<NamespaceObject> existingClasses = new List<NamespaceObject>();
        private List<NamespaceObject> existingInterfaces = new List<NamespaceObject>();
        private List<NamespaceObject> existingEnums = new List<NamespaceObject>();
#pragma warning restore IDE0044 // Add readonly modifier

        public NamespaceDocumentationGenerator(NamespaceDocumentation documentation, DirectoryInfo rootDirectory)
            : base(documentation, rootDirectory)
        {
        }

        public override FileInfo FileInfo =>
            new FileInfo($"{Path.Combine(this.RootDirectory.FullName, Path.Combine(this.Documentation.FullName.Split('.')))}.md");

        public override string DocumentationName => this.Documentation.FullName;

        public override string DocumentationType => "Namespace";

        public override async Task GenerateAsync()
        {
            await this.ParseExistingDocumentAsync();

            await base.GenerateAsync();

            await Task.WhenAll(from n in this.Documentation.Namespaces
                               select new NamespaceDocumentationGenerator(n, this.RootDirectory).GenerateAsync());

            await Task.WhenAll(from t in this.Documentation.Types
                               select new TypeDocumentationGenerator(t, this.RootDirectory).GenerateAsync());
        }

        protected override Task<MDDocument> GenerateDocumentAsync()
        {
            var document = new MDDocument();

            document.AddElement(this.GenerateDocumentHeader());

            var namespaces = (from n in this.Documentation.Namespaces
                              select this.GetNamespaceObject(n))
                              .Concat(this.existingNamespaces)
                              .Distinct()
                              .OrderBy(n => n.Name);

            if (namespaces.Any())
            {
                document.AddElement(
                    new MDH2
                    {
                        Text = "Namespaces",
                    });

                var table = new MDTable();
                table.Header.Cells.Add(new MDText
                {
                    Text = "Name",
                });

                foreach (var item in namespaces)
                {
                    var row = new MDTableRow();
                    row.Cells.Add(new MDLink
                    {
                        Text = item.Name,
                        Url = item.Url,
                    });
                    table.Rows.Add(row);
                }

                document.AddElement(table);
            }

            if (this.Documentation.Types.Any(t => t.IsClass))
            {
                document.AddElement(this.GetTable(
                    "Classes",
                    this.Documentation.Types.Where(t => t.IsClass)
                        .Select(t => this.GetNamespaceObject(t))
                        .Concat(this.existingClasses)
                        .Distinct()
                        .OrderBy(n => n.Name)));
            }

            if (this.Documentation.Types.Any(t => t.IsInterface))
            {
                document.AddElement(this.GetTable(
                    "Interfaces",
                    this.Documentation.Types.Where(t => t.IsInterface)
                        .Select(t => this.GetNamespaceObject(t))
                        .Concat(this.existingInterfaces)
                        .Distinct()
                        .OrderBy(n => n.Name)));
            }

            if (this.Documentation.Types.Any(t => t.IsEnum))
            {
                document.AddElement(this.GetTable(
                    "Enums",
                    this.Documentation.Types.Where(t => t.IsEnum)
                        .Select(t => this.GetNamespaceObject(t))
                        .Concat(this.existingEnums)
                        .Distinct()
                        .OrderBy(n => n.Name)));
            }

            return Task.FromResult(document);
        }

        private NamespaceObject GetNamespaceObject(NamespaceDocumentation namespaceDocumentation) =>
            new NamespaceObject
            {
                Name = namespaceDocumentation.FullName,
                Url = new NamespaceDocumentationGenerator(namespaceDocumentation, this.RootDirectory).RepoUrl,
            };

        private NamespaceObject GetNamespaceObject(TypeDocumentation typeDocumentation) =>
            new NamespaceObject
            {
                Name = typeDocumentation.Name,
                Summary = typeDocumentation.Summary?.ConvertToMDGroup(),
                Url = new TypeDocumentationGenerator(typeDocumentation, this.RootDirectory).RepoUrl,
            };

        private MDGroup GetTable(string header, IEnumerable<NamespaceObject> namespaceObject)
        {
            var group = new MDGroup();

            group.AddElement(
                new MDH2
                {
                    Text = header,
                });

            var table = new MDTable();
            table.Header.Cells.Add(new MDText
            {
                Text = "Name",
            });
            table.Header.Cells.Add(new MDText
            {
                Text = "Description",
            });

            foreach (var item in namespaceObject)
            {
                var row = new MDTableRow();
                row.Cells.Add(new MDLink
                {
                    Text = item.Name,
                    Url = item.Url,
                });
                row.Cells.Add(item.Summary);
                table.Rows.Add(row);
            }

            group.AddElement(table);

            return group;
        }

        private async Task ParseExistingDocumentAsync()
        {
            if (!this.FileInfo.Exists)
            {
                return;
            }

            using (var reader = this.FileInfo.OpenText())
            {
                string currentSection = null;
                List<NamespaceObject> existingObjects = null;

                while (reader.Peek() != -1)
                {
                    var line = await reader.ReadLineAsync();

                    // Begin a new section
                    if (line.StartsWith("## "))
                    {
                        this.SaveExisting(currentSection, existingObjects);

                        currentSection = line.Substring(3);
                        existingObjects = new List<NamespaceObject>();
                        continue;
                    }

                    // Parse for information.
                    if (line.StartsWith("|["))
                    {
                        var namespaceObject = new NamespaceObject
                        {
                            Name = ExistingNameRegex.Match(line).Value,
                            Url = ExistingUrlRegex.Match(line).Value,
                        };

                        var summary = line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

                        if (summary != null && !summary.StartsWith("["))
                        {
                            var group = new MDGroup();
                            group.AddElement(new MDText
                            {
                                Text = summary,
                            });

                            namespaceObject.Summary = group;
                        }

                        existingObjects.Add(namespaceObject);
                        continue;
                    }
                }

                this.SaveExisting(currentSection, existingObjects);
            }
        }

        private void SaveExisting(string currentSection, List<NamespaceObject> existingObjects)
        {
            if (currentSection == null || existingObjects == null)
            {
                return;
            }

            var fieldName = $"existing{currentSection}";

            var thisType = typeof(NamespaceDocumentationGenerator);
            var field = thisType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            field?.SetValue(this, existingObjects);
        }

        private class NamespaceObject
        {
            public string Name { get; set; }

            public MDGroup Summary { get; set; }

            public string Url { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is NamespaceObject namespaceObject)
                {
                    return this.Name == namespaceObject.Name;
                }

                return false;
            }

            public override int GetHashCode() =>
                this.Name.GetHashCode();
        }
    }
}
