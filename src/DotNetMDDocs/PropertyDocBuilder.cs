using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs
{
    public class PropertyDocBuilder : DocBuilder
    {
        private readonly PropertyDoc property;

        public PropertyDocBuilder(PropertyDoc property, TypeDoc type, Document document)
             : base(type, document)
        {
            this.property = property;
        }

        protected override string GetHeader()
        {
            return $"{type.Name}.{property.Name} Property";
        }
    }
}
