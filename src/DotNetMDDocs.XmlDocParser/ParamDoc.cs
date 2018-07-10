using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class ParamDoc
    {
        public string Name { get; private set; }
        public string Summary { get; private set; }
        public string Type { get; private set; }

        public ParamDoc(XElement xElement, string type)
        {
            Name = xElement.Attribute("name").Value;
            Summary = xElement.Value;
            Type = type;
        }
    }
}
