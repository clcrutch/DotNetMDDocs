// <copyright file="AssemblyDocumentation.cs" company="Chris Crutchfield">
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
using System.Xml.Linq;
using DotNetDocs.Extensions;
using DotNetDocs.Mixins.Contracts;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;

namespace DotNetDocs.ContainerDocumentations
{
    /// <summary>
    /// Parses a *.dll or *.exe file and generates it's documentation.
    /// </summary>
    public class AssemblyDocumentation : ContainerDocumentation, IDisposable
    {
        private readonly bool disposeAssemblyDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDocumentation"/> class.
        /// </summary>
        /// <param name="assemblyDefinition">The reference to the <see cref="AssemblyDefinition"/> that the instance of <see cref="AssemblyDocumentation"/> documents.</param>
        /// <param name="peFile">The reference to the <see cref="PEFile"/> that the instance of <see cref="AssemblyDocumentation"/> documents.</param>
        /// <param name="xDocument">The reference to the XML document that represents the XML comment documentation for the assembly.</param>
        /// <param name="disposeAssemblyDefinition">Indicates if <paramref name="assemblyDefinition"/> should be disposed with this object is disposed.</param>
        protected internal AssemblyDocumentation(AssemblyDefinition assemblyDefinition, PEFile peFile, XDocument xDocument, bool disposeAssemblyDefinition)
        {
            this.disposeAssemblyDefinition = disposeAssemblyDefinition;

            this.Decompiler = new CSharpDecompiler(assemblyDefinition.MainModule.FileName, new DecompilerSettings
            {
                DecompileMemberBodies = false,
                UsingStatement = false,
                UsingDeclarations = false,
                ShowXmlDocumentation = false,
                ThrowOnAssemblyResolveErrors = false,
            });

            this.AssemblyDefinition = assemblyDefinition;
            this.PEFile = peFile;

            this.Namespaces = this.GetNamespaceDocumentations(xDocument);

            this.DocumentationsToDispose = new List<AssemblyDocumentation>();
        }

        /// <summary>
        /// Gets the representation of the underlying assembly from Mono.Cecil.
        /// </summary>
        public AssemblyDefinition AssemblyDefinition { get; private set; }

        /// <inheritdoc />
        public override IEnumerable<IDocumentation> Children => this.Namespaces;

        /// <summary>
        /// Gets the file name for the underlying assembly file.
        /// </summary>
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(this.FilePath))
                {
                    return null;
                }

                return Path.GetFileName(this.FilePath);
            }
        }

        /// <summary>
        /// Gets the file path to the documented assembly.
        /// </summary>
        public string FilePath => this.AssemblyDefinition?.MainModule?.FileName;

        /// <inheritdoc />
        public override string FullName => this.AssemblyDefinition?.FullName;

        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        public override string Name => this.AssemblyDefinition?.Name?.Name;

        /// <summary>
        /// Gets a list of the namespaces contained within the assembly.
        /// </summary>
        public NamespaceDocumentation[] Namespaces { get; private set; }

        /// <summary>
        /// Gets an instance of the C# decompiler used to generate the declarations.
        /// </summary>
        protected internal CSharpDecompiler Decompiler { get; private set; }

        /// <summary>
        /// Gets a lists of the assembly documentations to dispose.
        /// </summary>
        protected internal List<AssemblyDocumentation> DocumentationsToDispose { get; private set; }

        /// <summary>
        /// Gets a representation of the underlying assembly from System.Reflection.Metadata.
        /// </summary>
        protected internal PEFile PEFile { get; private set; }

        /// <summary>
        /// Parses the assembly given its assembly and XML paths.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly in the file system.</param>
        /// <returns>The instance of <see cref="AssemblyDocumentation"/> that represents the assembly.</returns>
        public static AssemblyDocumentation Parse(string assemblyPath)
        {
            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));

            return Load(
                AssemblyDefinition.ReadAssembly(
                    assemblyPath,
                    new ReaderParameters
                    {
                        AssemblyResolver = assemblyResolver,
                        InMemory = true,
                    }),
                true);
        }

        /// <summary>
        /// Creates an <see cref="AssemblyDocumentation"/> using <paramref name="assemblyDefinition"/>.
        /// </summary>
        /// <param name="assemblyDefinition">The Mono.Cecil representation of the assembly.</param>
        /// <param name="disposeAssemblyDefinition">Indicates if <paramref name="assemblyDefinition"/> should be disposed with the returned <see cref="AssemblyDocumentation"/>.</param>
        /// <returns>A new instance of <see cref="AssemblyDocumentation"/> based off of <paramref name="assemblyDefinition"/>.</returns>
        public static AssemblyDocumentation Load(AssemblyDefinition assemblyDefinition, bool disposeAssemblyDefinition = false)
        {
            var assemblyPath = assemblyDefinition.MainModule.FileName;

            var xmlPath = Path.Combine(Path.GetDirectoryName(assemblyPath), $"{Path.GetFileNameWithoutExtension(assemblyPath)}.xml");

            PEFile peFile = null;
            var assemblyFileInfo = new FileInfo(assemblyPath);

            try
            {
                using (var stream = File.Open(assemblyFileInfo.FullName, FileMode.Open))
                {
                    peFile = new PEFile(assemblyFileInfo.Name, stream);
                }
            }
            catch (Exception)
            {
            }

            XDocument xDocument = null;
            if (File.Exists(xmlPath))
            {
                xDocument = XDocument.Load(xmlPath);
            }

            return new AssemblyDocumentation(
                    assemblyDefinition,
                    peFile,
                    xDocument,
                    disposeAssemblyDefinition);
        }

        /// <summary>
        /// Disposes of the current instance.
        /// </summary>
        public void Dispose()
        {
            if (this.disposeAssemblyDefinition)
            {
                this.AssemblyDefinition?.Dispose();
            }

            this.PEFile?.Dispose();

            foreach (var doc in this.DocumentationsToDispose)
            {
                doc.Dispose();
            }
        }

        private NamespaceDocumentation[] GetNamespaceDocumentations(XDocument xDocument) =>
            (from n in this.AssemblyDefinition.MainModule.GetRootNamespaces()
             where !n.Contains(".")
             select new NamespaceDocumentation(n, xDocument, this)).ToArray();
    }
}
