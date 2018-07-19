using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Mono.Cecil;

namespace DotNetDocs
{
    public class MethodDocumentation : DocumentationBase
    {
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

        public MethodDocumentation(MethodDefinition methodDefinition, XElement xElement)
            : base(methodDefinition, xElement)
        {
        }
    }
}
