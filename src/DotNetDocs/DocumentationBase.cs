// <copyright file="DocumentationBase.cs" company="Chris Crutchfield">
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
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetDocs
{
    public abstract class DocumentationBase
    {
        private string safeName;

        public DocumentationBase(IMemberDefinition memberDefinition, XElement xElement)
        {
            this.MemberDefinition = memberDefinition;
            this.XElement = xElement;
        }

        public virtual string Declaration { get; protected set; }

        public virtual string FullName => MemberDefinition?.FullName;

        public virtual string Name => MemberDefinition?.Name;

        public virtual string Remarks => XElement?.Descendants().FirstOrDefault(x => x.Name == "remarks")?.Value?.Trim();

        public virtual string Summary => XElement?.Descendants().FirstOrDefault(x => x.Name == "summary")?.Value?.Trim();

        public string SafeName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.safeName))
                {
                    return this.safeName;
                }

                this.safeName = this.Name;

                // Windows breaks if we don't keep the file name short.
                if (this.safeName.Length > 50)
                {
                    int hash = 0;
                    foreach (var c in this.safeName.ToCharArray())
                    {
                        hash += c;
                    }

                    hash %= 99999;

                    // Take the first part of the string so that we can recognize it.
                    // Hope that the hash is enough to make it unique.
                    this.safeName = $"{this.safeName.Substring(0, 45)}{hash}";
                }

                foreach (var invalid in Path.GetInvalidFileNameChars())
                {
                    this.safeName = this.safeName.Replace(invalid, '_');
                }

                this.safeName = this.safeName.Replace('(', '_');
                this.safeName = this.safeName.Replace(')', '_');

                return this.safeName;
            }
        }

        protected IMemberDefinition MemberDefinition { get; private set; }

        protected XElement XElement { get; private set; }
    }
}
