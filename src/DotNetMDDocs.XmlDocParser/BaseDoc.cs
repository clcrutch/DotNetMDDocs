using DotNetMDDocs.XmlDocParser.Extensions;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public abstract class BaseDoc
    {
        private string safeName;

        public virtual string Name { get; protected set; }
        public string Summary { get; protected set; }
        public string Remarks { get; protected set; }
        public string CodeSyntax { get; protected set; }

        public string SafeName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(safeName)) return safeName;

                safeName = Name;

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

        public BaseDoc(string identifier, XElement xElement, string baseName)
        {
            Name = xElement.Attribute("name").Value.Substring($"{identifier}:{baseName}".Length);

            Summary = (from e in xElement.Elements()
                       where e.Name == "summary"
                       select e.Value
                            .Replace(Environment.NewLine, " ")
                            .Replace("\n", " ")
                            .Trim()).SingleOrDefault();

            Remarks = (from e in xElement.Elements()
                       where e.Name == "remarks"
                       select e.Value
                            .Replace(Environment.NewLine, " ")
                            .Replace("\n", " ")
                            .Trim()).SingleOrDefault();
        }

        protected virtual string GetSyntaxAttributes(IMemberDefinition memberDefinition)
        {
            var stringBuilder = new StringBuilder();

            foreach (var attribute in memberDefinition.CustomAttributes)
            {
                stringBuilder.Append($"[{attribute.AttributeType.Name}");

                //if (attribute.HasConstructorArguments)
                //{
                //    var ctorArgs = (from c in attribute.ConstructorArguments
                //                    select c.ToCodeString()).ToArray();

                //    stringBuilder.Append($"({string.Join(", ", ctorArgs)})");
                //}

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }
    }
}
