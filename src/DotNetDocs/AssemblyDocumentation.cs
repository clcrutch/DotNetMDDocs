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

        public string FileName => AssemblyFileInfo?.Name;

        public string Name => AssemblyDefinition?.Name?.Name;

        public TypeDocumentation[] Types { get; private set; }

        public static AssemblyDocumentation Parse(string assemblyPath, string xmlPath)
        {
            return new AssemblyDocumentation(
                AssemblyDefinition.ReadAssembly(assemblyPath),
                XDocument.Load(xmlPath),
                new FileInfo(assemblyPath)
            );
        }

        protected AssemblyDocumentation(AssemblyDefinition assemblyDefinition, XDocument xDocument, FileInfo assemblyFileInfo)
        {
            AssemblyDefinition = assemblyDefinition;
            Types = GetTypeDocumentations(assemblyDefinition, xDocument);

            this.AssemblyFileInfo = assemblyFileInfo;
        }

        public void Dispose()
        {
            AssemblyDefinition?.Dispose();
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
