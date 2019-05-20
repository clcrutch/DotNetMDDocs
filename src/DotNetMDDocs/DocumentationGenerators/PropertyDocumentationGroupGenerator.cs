﻿// <copyright file="PropertyDocumentationGroupGenerator.cs" company="Chris Crutchfield">
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
using System.IO;
using DotNetDocs.MemberDocumentations;

namespace DotNetMDDocs.DocumentationGenerators
{
    internal class PropertyDocumentationGroupGenerator : MemberDocumentationGroupGenerator<PropertyDocumentation, PropertyDocumentationGenerator>
    {
        public PropertyDocumentationGroupGenerator(IEnumerable<PropertyDocumentation> documentations, DirectoryInfo rootDirectory, TypeDocumentationGenerator typeDocumentationGenerator)
            : base(documentations, rootDirectory, typeDocumentationGenerator)
        {
        }

        protected override string DocumentationsType => "Properties";
    }
}
