using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class FieldDoc : BaseDoc
    {
        public FieldDoc(XElement xElement, string baseName)
            : base("F", xElement, $"{baseName}.")
        {
        }
    }
}
