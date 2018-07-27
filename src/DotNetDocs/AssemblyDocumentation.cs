using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DotNetDocs
{
    public class AssemblyDocumentation : IDisposable
    {
        protected AssemblyDefinition AssemblyDefinition { get; private set; }

        protected FileInfo AssemblyFileInfo { get; private set; }

        public CSharpDecompiler Decompiler { get; private set; }

        public string FileName => AssemblyFileInfo?.Name;

        public string Name => AssemblyDefinition?.Name?.Name;

        public PEFile PEFile { get; private set; }

        public TypeDocumentation[] Types { get; private set; }

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
                assemblyFileInfo
            );
        }

        protected AssemblyDocumentation(AssemblyDefinition assemblyDefinition, PEFile peFile, XDocument xDocument, FileInfo assemblyFileInfo)
        {
            this.Decompiler = new CSharpDecompiler(assemblyFileInfo.FullName, new DecompilerSettings
            {
                DecompileMemberBodies = false,
                UsingStatement = false,
                UsingDeclarations = false,
                ShowXmlDocumentation = false
            });

            AssemblyDefinition = assemblyDefinition;
            this.AssemblyFileInfo = assemblyFileInfo;
            this.PEFile = peFile;

            Types = GetTypeDocumentations(assemblyDefinition, xDocument);
        }

        public void Dispose()
        {
            AssemblyDefinition?.Dispose();
            PEFile?.Dispose();
        }

        private TypeDocumentation[] GetTypeDocumentations(AssemblyDefinition assemblyDefinition, XDocument xDocument) => 
            (from t in assemblyDefinition.MainModule.Types
             where (t.Attributes & TypeAttributes.Public) == TypeAttributes.Public &&
                xDocument
                    .Descendants()
                    .Any(
                        d => d.Name == "member" &&
                        d.Attribute("name").Value.StartsWith("T:") &&
                        d.Attribute("name").Value.EndsWith(t.FullName)
                    )
             select new TypeDocumentation(
                        t,
                        xDocument
                            .Descendants()
                            .Single(
                                d => d.Name == "member" &&
                                d.Attribute("name").Value.StartsWith("T:") &&
                                d.Attribute("name").Value.EndsWith(t.FullName)
                        ),
                        this
                    )
            ).ToArray();
    }
}
