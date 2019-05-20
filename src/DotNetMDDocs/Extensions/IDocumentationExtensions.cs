// <copyright file="IDocumentationExtensions.cs" company="Chris Crutchfield">
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

using System.IO;
using DotNetDocs.Mixins.Contracts;

namespace DotNetMDDocs.Extensions
{
    public static class IDocumentationExtensions
    {
        public static string GetSafeName(this IDocumentation @this)
        {
            var safeName = @this.Name;

            // Windows breaks if we don't keep the file name short.
            if (safeName.Length > 50)
            {
                int hash = 0;
                foreach (var c in safeName.ToCharArray())
                {
                    hash += c;
                }

                hash %= 99999;

                // Take the first part of the string so that we can recognize it.
                // Hope that the hash is enough to make it unique.
                safeName = $"{safeName.Substring(0, 45)}{hash}";
            }

            foreach (var invalid in Path.GetInvalidFileNameChars())
            {
                safeName = safeName.Replace(invalid, '_');
            }

            safeName = safeName.Replace('(', '_');
            safeName = safeName.Replace(')', '_');

            return safeName;
        }
    }
}
