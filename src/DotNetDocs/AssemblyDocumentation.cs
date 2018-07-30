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
    public class AssemblyDocumentation : IDisposable
    {
        protected AssemblyDocumentation(AssemblyDefinition assemblyDefinition, PEFile peFile, XDocument xDocument, FileInfo assemblyFileInfo)
        {
            this.Decompiler = new CSharpDecompiler(assemblyFileInfo.FullName, new DecompilerSettings
            {
                DecompileMemberBodies = false,
                UsingStatement = false,
                UsingDeclarations = false,
                ShowXmlDocumentation = false,
            });

            this.AssemblyDefinition = assemblyDefinition;
            this.AssemblyFileInfo = assemblyFileInfo;
            this.PEFile = peFile;

            this.Types = this.GetTypeDocumentations(assemblyDefinition, xDocument);
        }

        public CSharpDecompiler Decompiler { get; private set; }

        public string FileName => AssemblyFileInfo?.Name;

        public string Name => AssemblyDefinition?.Name?.Name;

        public PEFile PEFile { get; private set; }

        public TypeDocumentation[] Types { get; private set; }

        protected AssemblyDefinition AssemblyDefinition { get; private set; }

        protected FileInfo AssemblyFileInfo { get; private set; }

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
