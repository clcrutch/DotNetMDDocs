// <copyright file="Document.cs" company="Chris Crutchfield">
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

using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetMDDocs.XmlDocParser
{
    public class Document
    {
        public Document(string filepath, string dllPath)
        {
            var xDocument = XDocument.Load(filepath);

            using (var assembly = AssemblyDefinition.ReadAssembly(dllPath))
            {
                this.Assembly = this.ParseAssemblyDoc(xDocument);
                this.Types = this.ParseTypeDocs(xDocument, assembly);
            }
        }

        public AssemblyDoc Assembly { get; set; }

        public TypeDoc[] Types { get; set; }

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

        private TypeDoc[] ParseTypeDocs(XDocument xDocument, AssemblyDefinition assembly)
        {
            var members = (from e in xDocument.Root.Elements()
                           where e.Name == "members"
                           select e).Single();

            var typeMembers = from e in members.Elements()
                              where e.Name == "member" && e.Attribute("name").Value.StartsWith("T:")
                              select e;

            var types = from t in typeMembers
                        select new TypeDoc(t, xDocument, assembly);

            return types.ToArray();
        }
    }
}
