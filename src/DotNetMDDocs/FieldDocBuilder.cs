using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs
{
    public class FieldDocBuilder : DocBuilder
    {
        private readonly FieldDoc field;

        public FieldDocBuilder(FieldDoc field, TypeDoc type, Document document)
             : base(type, document)
        {
            this.field = field;
        }

        protected override string GetHeader()
        {
            return $"{type.Name}.{field.Name} Field";
        }
    }
}
