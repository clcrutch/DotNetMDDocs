// <copyright file="TypeDeclarationProvider.cs" company="Chris Crutchfield">
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

using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;

namespace DotNetDocs.Mixins
{
    /// <summary>
    /// A declaration provider for types.
    /// </summary>
    internal class TypeDeclarationProvider : DeclarationProvider
    {
        private readonly string typeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDeclarationProvider"/> class.
        /// </summary>
        /// <param name="decompiler">An instance of the <see cref="CSharpDecompiler"/> to use for generating declarations.</param>
        /// <param name="typeName">The name of the type to pass to <paramref name="decompiler"/>, used to decompile generate the declaration.</param>
        public TypeDeclarationProvider(CSharpDecompiler decompiler, string typeName)
            : base(decompiler)
        {
            this.typeName = typeName;
        }

        /// <inheritdoc />
        public override string GetDeclaration()
        {
            string declaration = null;
            lock (DecompilerLock)
            {
                declaration = this.PrettyifyDeclaration(this.Decompiler.DecompileTypeAsString(new FullTypeName(this.typeName)));
            }

            declaration = declaration.Substring(declaration.IndexOf('{') + 1);
            declaration = declaration.Substring(0, declaration.IndexOf('{')).Trim();

            return declaration;
        }
    }
}
