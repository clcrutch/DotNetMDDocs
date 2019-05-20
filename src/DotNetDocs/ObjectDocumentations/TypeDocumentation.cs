// <copyright file="TypeDocumentation.cs" company="Chris Crutchfield">
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
using System.Linq;
using System.Xml.Linq;
using DotNetDocs.ContainerDocumentations;
using DotNetDocs.Extensions;
using DotNetDocs.MemberDocumentations;
using DotNetDocs.Mixins;
using DotNetDocs.Mixins.Contracts;
using DotNetDocs.Mono.Cecil.Extensions;
using Mono.Cecil;
using Serilog;
using TypeDefinition = Mono.Cecil.TypeDefinition;

namespace DotNetDocs.ObjectDocumentations
{
    /// <summary>
    /// Parses a type.
    /// </summary>
    public class TypeDocumentation : ObjectDocumentation, IContainerDocumentation
    {
        private readonly ContainerDocumentationMixin containerDocumentationMixin;
        private readonly TypeDefinition typeDefinition;

        private TypeDocumentation baseType;
        private bool hasGottenBaseType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDocumentation"/> class.
        /// </summary>
        /// <param name="typeDefinition">The <see cref="TypeDefinition"/> which to document.</param>
        /// <param name="xElement">The XML element representing the XML comments for the current type.</param>
        /// <param name="namespaceDocumentation">The parent <see cref="NamespaceDocumentation"/> for the type.</param>
        /// <param name="declaringAssembly">Type assembly which declares this type.</param>
        protected internal TypeDocumentation(TypeDefinition typeDefinition, XElement xElement, NamespaceDocumentation namespaceDocumentation, AssemblyDocumentation declaringAssembly)
        {
            Log.Verbose("Creating documentation for {typeName}", typeDefinition.Name);

            this.ObjectDocumentationMixin = new ObjectDocumentationMixin(typeDefinition, xElement, this, new TypeDeclarationProvider(declaringAssembly.Decompiler, typeDefinition.FullName));

            this.typeDefinition = typeDefinition;

            this.DeclaringAssembly = declaringAssembly;
            this.NamespaceDocumentation = namespaceDocumentation;

            if (declaringAssembly.PEFile != null)
            {
                var typeDefs = from t in declaringAssembly.PEFile.Metadata.TypeDefinitions
                               select declaringAssembly.PEFile.Metadata.GetTypeDefinition(t);

                this.ReflectionTypeDefinition = (from t in typeDefs
                                                 where declaringAssembly.PEFile.Metadata.GetString(t.Namespace) == namespaceDocumentation.FullName &&
                                                    declaringAssembly.PEFile.Metadata.GetString(t.Name) == this.Name
                                                 select t).Single();
            }

            this.ConstructorDocumentations = this.GetConstructorDocumentations(typeDefinition, xElement?.Document);
            this.FieldDocumentations = this.GetFieldDocumentations(typeDefinition, xElement?.Document);
            this.PropertyDocumentations = this.GetPropertyDocumentations(typeDefinition, xElement?.Document);
            this.MethodDocumentations = this.GetMethodDocumentations(typeDefinition, xElement?.Document);

            this.containerDocumentationMixin = new ContainerDocumentationMixin(this);
        }

        /// <summary>
        /// Gets the children of the current type.  This includes fields, constructors, properties, and methods.
        /// </summary>
        IEnumerable<IDocumentation> IContainerDocumentation.Children => new List<IDocumentation>()
                                                                            .ConcatNull(this.FieldDocumentations)
                                                                            .ConcatNull(this.PropertyDocumentations)
                                                                            .ConcatNull(this.MethodDocumentations)
                                                                            .ConcatNull(this.ConstructorDocumentations);

        /// <summary>
        /// Gets the base type for the type.
        /// </summary>
        public TypeDocumentation BaseType
        {
            get
            {
                if (!this.hasGottenBaseType)
                {
                    this.baseType = this.GetBaseType();
                    this.hasGottenBaseType = true;
                }

                return this.baseType;
            }
        }

        /// <summary>
        /// Gets a list of constructors for the current type.
        /// </summary>
        public MethodDocumentation[] ConstructorDocumentations { get; private set; }

        /// <summary>
        /// Gets the assembly that declares the current type.
        /// </summary>
        public AssemblyDocumentation DeclaringAssembly { get; private set; }

        /// <summary>
        /// Gets a list of fields for the current type.
        /// </summary>
        public FieldDocumentation[] FieldDocumentations { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the type is a class.
        /// </summary>
        public bool IsClass => this.typeDefinition?.IsClass ?? false;

        /// <summary>
        /// Gets a value indicating whether the type is a enum.
        /// </summary>
        public bool IsEnum => this.typeDefinition?.IsEnum ?? false;

        /// <summary>
        /// Gets a value indicating whether the type is an interface.
        /// </summary>
        public bool IsInterface => this.typeDefinition?.IsInterface ?? false;

        /// <summary>
        /// Gets a list of methods for the current type.
        /// </summary>
        public MethodDocumentation[] MethodDocumentations { get; private set; }

        /// <summary>
        /// Gets the parent namespace.
        /// </summary>
        public NamespaceDocumentation NamespaceDocumentation { get; private set; }

        /// <summary>
        /// Gets a list of the properties for the current type.
        /// </summary>
        public PropertyDocumentation[] PropertyDocumentations { get; private set; }

        /// <summary>
        /// Gets the underlying <see cref="System.Reflection.Metadata.TypeDefinition"/> for the type.
        /// </summary>
        protected internal System.Reflection.Metadata.TypeDefinition ReflectionTypeDefinition { get; private set; }

        /// <summary>
        /// Gets the member referenced by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The full name of the member to return.</param>
        /// <returns>The member referenced by <paramref name="fullName"/>.</returns>
        public IDocumentation this[string fullName] => this.containerDocumentationMixin[fullName];

        /// <summary>
        /// Gets a value indicating whether this type contains a member referenced by <paramref name="fullName"/>.
        /// </summary>
        /// <param name="fullName">The full name of the member to find.</param>
        /// <returns>A value indicating if the type contains the member referenced by <paramref name="fullName"/>.</returns>
        public bool Contains(string fullName) => this.containerDocumentationMixin.Contains(fullName);

        private TypeDocumentation GetBaseType()
        {
            var baseTypeDefinition = this.typeDefinition.BaseType?.Resolve();
            if (baseTypeDefinition == null)
            {
                return null;
            }

            if (this.DeclaringAssembly.Contains(baseTypeDefinition.FullName))
            {
                return this.DeclaringAssembly[baseTypeDefinition.FullName] as TypeDocumentation;
            }
            else
            {
                if (baseTypeDefinition.Module.Assembly.FullName == this.DeclaringAssembly.FullName)
                {
                    throw new NotImplementedException();
                }

                var declaringAssembly = AssemblyDocumentation.Load(baseTypeDefinition.Module.Assembly);

                // Add this to the list we need to cleanup later.
                this.DeclaringAssembly.DocumentationsToDispose.Add(declaringAssembly);

                return declaringAssembly[baseTypeDefinition.FullName] as TypeDocumentation;
            }
        }

        private MethodDocumentation[] GetConstructorDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from m in typeDefinition.Methods
             where m.IsConstructor && !m.IsPrivate
             select new MethodDocumentation(
                 m,
                 m.ToXelement(xDocument),
                 m.ToEntityHandle(this.DeclaringAssembly.PEFile, this.DeclaringAssembly.Decompiler),
                 this))
            .ToArray();

        private FieldDocumentation[] GetFieldDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from f in typeDefinition.Fields
             where !f.IsPrivate
             select new FieldDocumentation(
                 f,
                 f.ToXElement(xDocument),
                 f.ToEntityHandle(this.DeclaringAssembly.PEFile),
                 this))
             .ToArray();

        private MethodDocumentation[] GetMethodDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from m in typeDefinition.Methods
             where !m.IsConstructor &&
                    !m.IsPrivate &&
                    (m.Attributes & MethodAttributes.SpecialName) == 0
             select new MethodDocumentation(
                 m,
                 m.ToXelement(xDocument),
                 m.ToEntityHandle(this.DeclaringAssembly.PEFile, this.DeclaringAssembly.Decompiler),
                 this))
             .ToArray();

        private PropertyDocumentation[] GetPropertyDocumentations(TypeDefinition typeDefinition, XDocument xDocument) =>
            (from p in typeDefinition.Properties
             where !(p.GetMethod?.IsPrivate ?? true) || !(p.SetMethod?.IsPrivate ?? true)
             select new PropertyDocumentation(
                p,
                p.ToXElement(xDocument),
                p.ToEntityHandle(this.DeclaringAssembly.PEFile),
                this))
             .ToArray();
    }
}
