using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMDDocs.XmlDocParser.Extensions
{
    public static class TypeReferenceExtensions
    {
        public static string DisplayName(this TypeReference typeReference)
        {
            var genericInstanceType = typeReference as GenericInstanceType;

            if (genericInstanceType?.HasGenericArguments ?? false)
            {
                var typeParams = (from t in genericInstanceType.GenericArguments
                                  select t.DisplayName()).ToArray();

                return $"{typeReference.Name.Substring(0, typeReference.Name.IndexOf('`'))}<{string.Join(", ", typeParams)}>";
            }
            else
                return typeReference.Name;
        }
    }
}
