// <copyright file="DeclarationProvider.cs" company="Chris Crutchfield">
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
using DotNetDocs.Extensions;
using DotNetDocs.Mixins.Contracts;
using ICSharpCode.Decompiler.CSharp;

namespace DotNetDocs.Mixins
{
    /// <summary>
    /// Represetns a generic declaration provider.
    /// </summary>
    internal abstract class DeclarationProvider : IDeclarationProvider
    {
        /// <summary>
        /// The lock for the decompiler.
        /// </summary>
        protected static readonly object DecompilerLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="DeclarationProvider"/> class.
        /// </summary>
        /// <param name="decompiler">The <see cref="ICSharpCode"/> to use for decompiling.</param>
        protected DeclarationProvider(CSharpDecompiler decompiler)
        {
            this.Decompiler = decompiler;
        }

        /// <summary>
        /// Gets the decompiler for the declaration provider.
        /// </summary>
        protected CSharpDecompiler Decompiler { get; private set; }

        /// <summary>
        /// In a extended type, returns a string declaration for using the current provider.
        /// </summary>
        /// <returns>A string representing the declaration.</returns>
        public abstract string GetDeclaration();

        /// <summary>
        /// Cleans up the decompiled <paramref name="declaration"/>.
        /// </summary>
        /// <param name="declaration">The declaration to cleanup.</param>
        /// <returns>The cleaner <paramref name="declaration"/>.</returns>
        protected string PrettyifyDeclaration(string declaration) =>
            declaration
                .Replace("[System.Runtime.CompilerServices.CompilerGenerated]", string.Empty)
                .TrimAndCombine(Environment.NewLine);
    }
}
