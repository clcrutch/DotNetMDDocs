// <copyright file="ReturnValueDocumentation.cs" company="Chris Crutchfield">
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
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetDocs
{
    /// <summary>
    /// Parses a return value.
    /// </summary>
    public class ReturnValueDocumentation : MethodInputOutputDocumentation
    {
        private readonly MethodReturnType methodReturnType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnValueDocumentation"/> class.
        /// </summary>
        /// <param name="methodReturnType">The <see cref="MethodReturnType"/> which to document.</param>
        /// <param name="xElement">The XML element representing the XML comments for the current member.</param>
        public ReturnValueDocumentation(MethodReturnType methodReturnType, XElement xElement)
            : base(xElement)
        {
            this.methodReturnType = methodReturnType;
        }

        /// <inheritdoc />
        public override string TypeName => this.methodReturnType.ReturnType.Name;
    }
}
