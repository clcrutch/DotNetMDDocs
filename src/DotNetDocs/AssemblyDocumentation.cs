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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;

namespace DotNetDocs
{
    /// <summary>
    /// Parses a *.dll or *.exe file and generates it's documentation.
    /// </summary>
    public class AssemblyDocumentation : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDocumentation"/> class.
        /// </summary>
        /// <param name="assemblyDefinition">The reference to the <see cref="AssemblyDefinition"/> that the instance of <see cref="AssemblyDocumentation"/> documents.</param>
        /// <param name="peFile">The reference to the <see cref="PEFile"/> that the instance of <see cref="AssemblyDocumentation"/> documents.</param>
        /// <param name="xDocument">The reference to the XML document that represents the XML comment documentation for the assembly.</param>
        /// <param name="assemblyFileInfo">A <see cref="FileInfo"/> object that points to the assembly in the file system.</param>
        protected AssemblyDocumentation(AssemblyDefinition assemblyDefinition, PEFile peFile, XDocument xDocument, FileInfo assemblyFileInfo)
        {
            this.Decompiler = new CSharpDecompiler(assemblyFileInfo.FullName, new DecompilerSettings
            {
                DecompileMemberBodies = false,
                UsingStatement = false,
                UsingDeclarations = false,
                ShowXmlDocumentation = false,
                ThrowOnAssemblyResolveErrors = false,
            });

            this.AssemblyDefinition = assemblyDefinition;
            this.AssemblyFileInfo = assemblyFileInfo;
            this.PEFile = peFile;

            this.Types = this.GetTypeDocumentations(assemblyDefinition, xDocument);
        }

        /// <summary>
        /// Gets an instance of the C# decompiler used to generate the declarations.
        /// </summary>
        public CSharpDecompiler Decompiler { get; private set; }

        /// <summary>
        /// Gets the file name for the underlying assembly file.
        /// </summary>
        public string FileName => this.AssemblyFileInfo?.Name;

        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        public string Name => this.AssemblyDefinition?.Name?.Name;

        /// <summary>
        /// Gets a representation of the underlying assembly from System.Reflection.Metadata.
        /// </summary>
        public PEFile PEFile { get; private set; }

        /// <summary>
        /// Gets a list of all of the documented types.
        /// </summary>
        public TypeDocumentation[] Types { get; private set; }

        /// <summary>
        /// Gets the representation of the underlying assembly from Mono.Cecil.
        /// </summary>
        protected AssemblyDefinition AssemblyDefinition { get; private set; }

        /// <summary>
        /// Gets the <see cref="FileInfo"/> for the underlying assembly.
        /// </summary>
        protected FileInfo AssemblyFileInfo { get; private set; }

        /// <summary>
        /// Parses the assembly given its assembly and XML paths.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly in the file system.</param>
        /// <param name="xmlPath">The path to the XML representing the XML comments in the file system.</param>
        /// <returns>The instance of <see cref="AssemblyDocumentation"/> that represents the assembly.</returns>
        public static AssemblyDocumentation Parse(string assemblyPath, string xmlPath)
        {
            PEFile peFile = null;
            var assemblyFileInfo = new FileInfo(assemblyPath);

            using (var stream = File.Open(assemblyFileInfo.FullName, FileMode.Open))
            {
                peFile = new PEFile(assemblyFileInfo.Name, stream);
            }

            return new AssemblyDocumentation(
                AssemblyDefinition.ReadAssembly(assemblyPath),
                peFile,
                XDocument.Load(xmlPath),
                assemblyFileInfo);
        }

        /// <summary>
        /// Disposes of the current instance.
        /// </summary>
        public void Dispose()
        {
            this.AssemblyDefinition?.Dispose();
            this.PEFile?.Dispose();
        }

        private TypeDocumentation[] GetTypeDocumentations(AssemblyDefinition assemblyDefinition, XDocument xDocument) =>
            (from t in assemblyDefinition.MainModule.Types
             where (t.Attributes & TypeAttributes.Public) == TypeAttributes.Public &&
                xDocument
                    .Descendants()
                    .Any(
                        d => d.Name == "member" &&
                        d.Attribute("name").Value.StartsWith("T:") &&
                        d.Attribute("name").Value.EndsWith(t.FullName))
             select new TypeDocumentation(
                        t,
                        xDocument
                            .Descendants()
                            .Single(
                                d => d.Name == "member" &&
                                d.Attribute("name").Value.StartsWith("T:") &&
                                d.Attribute("name").Value.EndsWith(t.FullName)),
                        this))
            .ToArray();
    }
}
