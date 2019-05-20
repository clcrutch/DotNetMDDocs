// <copyright file="FieldDeclarationProvider.cs" company="Chris Crutchfield">
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

using System.Reflection.Metadata;
using ICSharpCode.Decompiler.CSharp;

namespace DotNetDocs.Mixins
{
    /// <summary>
    /// A declaration provider for fields that trims '=' from generated declarations.
    /// </summary>
    internal class FieldDeclarationProvider : SimpleDeclarationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeclarationProvider"/> class.
        /// </summary>
        /// <param name="decompiler">An instance of the <see cref="CSharpDecompiler"/> to use for generating declarations.</param>
        /// <param name="entityHandle">The entity handle of the member to pass to <paramref name="decompiler"/>, used to decompile generate the declaration.</param>
        public FieldDeclarationProvider(CSharpDecompiler decompiler, EntityHandle? entityHandle)
            : base(decompiler, entityHandle)
        {
        }

        /// <inheritdoc />
        public override string GetDeclaration()
        {
            var declaration = base.GetDeclaration();

            if (declaration?.Contains("=") ?? false)
            {
                declaration = $"{declaration.Substring(0, declaration.IndexOf('=')).Trim()};";
            }

            return declaration;
        }
    }
}
