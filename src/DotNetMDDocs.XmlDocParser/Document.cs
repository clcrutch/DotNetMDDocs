using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DotNetMDDocs.XmlDocParser
{
    public class Document
    {
        public AssemblyDoc Assembly { get; set; }
        public TypeDoc[] Types { get; set; }

        public Document(string filepath)
        {
            var xDocument = XDocument.Load(filepath);

            Assembly = ParseAssemblyDoc(xDocument);
            Types = ParseTypeDocs(xDocument);
        }

        private AssemblyDoc ParseAssemblyDoc(XDocument xDocument)
        {
            var assemblyDoc = new AssemblyDoc
            {
                Name = (from e in xDocument.Root.Elements()
                        where e.Name == "assembly"
                        select e.Value).Single()
            };

            return assemblyDoc;
        }

        private TypeDoc[] ParseTypeDocs(XDocument xDocument)
        {
            var members = (from e in xDocument.Root.Elements()
                           where e.Name == "members"
                           select e).Single();

            var typeMembers = from e in members.Elements()
                              where e.Name == "member" && e.Attribute("name").Value.StartsWith("T:")
                              select e;

            var types = from t in typeMembers
                        select new TypeDoc(t, xDocument);

            return types.ToArray();
        }
    }
}
