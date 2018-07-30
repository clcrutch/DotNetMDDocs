﻿// <copyright file="MethodDocumentation.cs" company="Chris Crutchfield">
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
using System.Reflection.Metadata;
using System.Xml.Linq;

using MethodDefinition = Mono.Cecil.MethodDefinition;

namespace DotNetDocs
{
    public class MethodDocumentation : DocumentationBase
    {
        public MethodDocumentation(MethodDefinition methodDefinition, XElement xElement, EntityHandle handle, TypeDocumentation declaringType)
            : base(methodDefinition, xElement)
        {
            this.DeclaringType = declaringType;

            var declaringAssembly = declaringType.DeclaringAssembly;
            this.Declaration = declaringAssembly.Decompiler.DecompileAsString(handle).Trim();
        }

        public override string Name
        {
            get
            {
                var parameters = (from p in this.MethodDefinition.Parameters
                                  select p.ParameterType.Name).ToArray();

                return $"{base.Name}({string.Join(", ", parameters)})";
            }
        }

        public bool IsConstructor => MethodDefinition?.IsConstructor ?? false;

        protected TypeDocumentation DeclaringType { get; private set; }

        protected MethodDefinition MethodDefinition => (MethodDefinition)MemberDefinition;
    }
}