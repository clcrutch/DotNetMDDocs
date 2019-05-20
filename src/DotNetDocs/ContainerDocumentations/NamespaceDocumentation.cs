// <copyright file="NamespaceDocumentation.cs" company="Chris Crutchfield">
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

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DotNetDocs.Extensions;
using DotNetDocs.Mixins.Contracts;
using DotNetDocs.ObjectDocumentations;
using Mono.Cecil;

namespace DotNetDocs.ContainerDocumentations
{
    /// <summary>
    /// Documentation for a namespace.
    /// </summary>
    public class NamespaceDocumentation : ContainerDocumentation
    {
        private readonly string @namespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceDocumentation"/> class.
        /// </summary>
        /// <param name="namespace">The namespace to document.</param>
        /// <param name="xDocument">The entire XML document for the assembly.</param>
        /// <param name="declaringAssembly">The <see cref="AssemblyDefinition"/> which contains the namespace.</param>
        internal NamespaceDocumentation(string @namespace, XDocument xDocument, AssemblyDocumentation declaringAssembly)
        {
            this.@namespace = @namespace;
            this.Namespaces = this.GetNamespaceDocumentations(declaringAssembly, @namespace, xDocument);
            this.Types = this.GetTypeDocumentations(declaringAssembly, @namespace, xDocument);
        }

        /// <inheritdoc />
        public override IEnumerable<IDocumentation> Children => this.Namespaces.Cast<IDocumentation>().Concat(this.Types);

        /// <inheritdoc />
        public override string Name => this.FullName.Substring(this.FullName.LastIndexOf('.') + 1);

        /// <inheritdoc />
        public override string FullName => this.@namespace;

        /// <summary>
        /// Gets a list of namespaces contained under the current namespace.
        /// </summary>
        public NamespaceDocumentation[] Namespaces { get; private set; }

        /// <summary>
        /// Gets a list of all of the documented types in the namespace.
        /// </summary>
        public TypeDocumentation[] Types { get; private set; }

        private NamespaceDocumentation[] GetNamespaceDocumentations(AssemblyDocumentation declaringAssembly, string @namespace, XDocument xDocument) =>
            (from n in declaringAssembly.AssemblyDefinition.MainModule.GetNamespaces()
             where n.StartsWith(@namespace) &&
                n != @namespace &&
                n.Count(x => x == '.') == @namespace.Count(x => x == '.') + 1
             select new NamespaceDocumentation(n, xDocument, declaringAssembly)).ToArray();

        private TypeDocumentation[] GetTypeDocumentations(AssemblyDocumentation declaringAssembly, string @namespace, XDocument xDocument) =>
            (from t in declaringAssembly.AssemblyDefinition.MainModule.Types
             where (t.Attributes & TypeAttributes.Public) == TypeAttributes.Public &&
                t.Namespace == @namespace
             select new TypeDocumentation(
                        t,
                        this.GetTypeXmlElement(t, xDocument),
                        this,
                        declaringAssembly))
            .ToArray();

        private XElement GetTypeXmlElement(TypeDefinition typeDefinition, XDocument xDocument) =>
            xDocument?.Descendants()?.SingleOrDefault(
                d => d.Name == "member" &&
                d.Attribute("name").Value.StartsWith("T:") &&
                d.Attribute("name").Value.EndsWith(typeDefinition.FullName));
    }
}
