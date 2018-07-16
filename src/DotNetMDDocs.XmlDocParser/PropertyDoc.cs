using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class PropertyDoc : BaseDoc
    { 
        public PropertyDoc(XElement xElement, string baseName, TypeDefinition typeDefinition)
            : base("P", xElement, $"{baseName}.")
        {
            var propertyDefinition = typeDefinition?.Properties.SingleOrDefault(p => p.Name == Name);

            CodeSyntax = GetCodeSyntax(propertyDefinition);
        }

        private string GetCodeSyntax(PropertyDefinition propertyDefinition)
        {
            if (propertyDefinition == null)
                return string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(GetSyntaxAttributes(propertyDefinition));

            stringBuilder.Append("public ");
            stringBuilder.Append(propertyDefinition.PropertyType.Name);
            stringBuilder.Append(" ");
            stringBuilder.Append(propertyDefinition.Name);
            stringBuilder.Append(" { ");
            if (propertyDefinition.GetMethod?.IsPublic ?? false)
                stringBuilder.Append("get; ");
            if (propertyDefinition.SetMethod?.IsPublic ?? false)
                stringBuilder.Append("set; ");
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}
