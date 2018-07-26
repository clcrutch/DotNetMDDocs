using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetDocs
{
    public abstract class DocumentationBase
    {
        private string safeName;

        protected IMemberDefinition MemberDefinition { get; private set; }
        protected XElement XElement { get; private set; }

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

        public DocumentationBase(IMemberDefinition memberDefinition, XElement xElement)
        {
            MemberDefinition = memberDefinition;
            XElement = xElement;
        }
    }
}
