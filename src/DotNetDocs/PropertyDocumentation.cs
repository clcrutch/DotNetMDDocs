using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

using PropertyDefinition = Mono.Cecil.PropertyDefinition;

namespace DotNetDocs
{
    public class PropertyDocumentation : DocumentationBase
    {
        protected TypeDocumentation DeclaringType { get; private set; }

        public PropertyDocumentation(PropertyDefinition propertyDefinition, XElement xElement, EntityHandle handle, TypeDocumentation declaringType)
            : base(propertyDefinition, xElement)
        {
            DeclaringType = declaringType;

            var declaringAssembly = declaringType.DeclaringAssembly;
            Declaration = declaringAssembly.Decompiler.DecompileAsString(handle).Trim();
        }
    }
}
