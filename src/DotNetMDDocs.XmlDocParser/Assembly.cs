using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DotNetMDDocs.XmlDocParser
{
    public class AssemblyDoc
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }
}
