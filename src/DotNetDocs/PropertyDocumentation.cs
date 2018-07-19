using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetDocs
{
    public class PropertyDocumentation : DocumentationBase
    {
        public PropertyDocumentation(PropertyDefinition propertyDefinition, XElement xElement)
            : base(propertyDefinition, xElement)
        {
        }
    }
}
