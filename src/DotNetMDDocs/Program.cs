using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DotNetMDDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            var document = new Document(args[0]);

            var docs = Directory.CreateDirectory("docs");

            foreach (var type in document.Types)
            {
                var rootDir = Directory.CreateDirectory(Path.Combine(docs.FullName, Path.Combine(type.Namespace.Split('.'))));
                var typeDir = new DirectoryInfo(Path.Combine(rootDir.FullName, type.SafeName));

                if (typeDir.Exists)
                    typeDir.Delete(true);

                typeDir.Create();

                var typeDocBuilder = new TypeDocBuilder(type, document);
                using (var stream = File.CreateText(Path.Combine(rootDir.FullName, $"{type.SafeName}.md")))
                {
                    stream.Write(typeDocBuilder.Generate());
                }

                // Constructors
                GenerateDocs<MethodDocBuilder>(type.Constructors, type, document, typeDir, "Constructors");

                // Properties
                GenerateDocs<PropertyDocBuilder>(type.Properties, type, document, typeDir, "Properties");

                // Methods
                GenerateDocs<MethodDocBuilder>(type.Methods, type, document, typeDir, "Methods");

                // Fields
                GenerateDocs<FieldDocBuilder>(type.Fields, type, document, typeDir, "Fields");
            }

#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
#endif
        }

        private static void GenerateDocs<TBuilder>(IEnumerable<BaseDoc> docs, TypeDoc type, Document document, DirectoryInfo typeDir, string dirName)
            where TBuilder : DocBuilder
        {
            var docDir = Directory.CreateDirectory(Path.Combine(typeDir.FullName, dirName));
            foreach (var doc in docs)
            {
                var docBuilder = (TBuilder)Activator.CreateInstance(typeof(TBuilder), doc, type, document);
                using (var stream = File.CreateText(Path.Combine(docDir.FullName, $"{doc.SafeName}.md")))
                {
                    stream.Write(docBuilder.Generate());
                }
            }
        }
    }
}
