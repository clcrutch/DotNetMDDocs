using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.XmlDocParser.Extensions
{
    public static class CustomAttributeArgumentExtensions
    {
        public static string ToCodeString(this CustomAttributeArgument @this)
        {
            if (@this.Type.FullName == "System.String")
                return $"\"{@this.Value}\"";
            return @this.Value.ToString();
        }
    }
}
