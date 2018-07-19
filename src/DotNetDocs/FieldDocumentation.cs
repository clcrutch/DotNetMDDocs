using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetDocs
{
    public class FieldDocumentation : DocumentationBase
    {
        public FieldDocumentation(FieldDefinition fieldDefinition, XElement xElement)
            : base(fieldDefinition, xElement)
        {
        }
    }
}
