using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class PropertyDoc : BaseDoc
    { 
        public PropertyDoc(XElement xElement, string baseName)
            : base("P", xElement, $"{baseName}.")
        {
        }
    }
}
