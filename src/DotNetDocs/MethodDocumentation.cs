using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

using MethodDefinition = Mono.Cecil.MethodDefinition;

namespace DotNetDocs
{
    public class MethodDocumentation : DocumentationBase
    {
        protected TypeDocumentation DeclaringType { get; private set; }
        protected MethodDefinition MethodDefinition => (MethodDefinition)MemberDefinition;

        public override string Name
        {
            get
            {
                var parameters = (from p in MethodDefinition.Parameters
                                  select p.ParameterType.Name).ToArray();

                return $"{base.Name}({string.Join(", ", parameters)})";
            }
        }

        public bool IsConstructor => MethodDefinition?.IsConstructor ?? false;

        public MethodDocumentation(MethodDefinition methodDefinition, XElement xElement, EntityHandle handle, TypeDocumentation declaringType)
            : base(methodDefinition, xElement)
        {
            DeclaringType = declaringType;

            var declaringAssembly = declaringType.DeclaringAssembly;
            Declaration = declaringAssembly.Decompiler.DecompileAsString(handle).Trim();
        }
    }
}
