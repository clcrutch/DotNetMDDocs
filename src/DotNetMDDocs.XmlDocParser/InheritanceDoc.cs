using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.XmlDocParser
{
    public class InheritanceDoc
    {
        public string Namespace { get; set; }
        public string Name { get; set; }

        public InheritanceDoc BaseClass { get; set; }
    }
}
