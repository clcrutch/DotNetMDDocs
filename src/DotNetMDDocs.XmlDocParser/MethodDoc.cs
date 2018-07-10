using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class MethodDoc : BaseDoc
    {
        public bool IsConstructor { get; private set; }
        public IEnumerable<ParamDoc> Params { get; private set; }

        public MethodDoc(XElement xElement, string baseName)
            : base("M", xElement, $"{baseName}.")
        {
            if (!Name.EndsWith(")"))
                Name = $"{Name}()";

            if (Name.Contains("#ctor"))
            {
                Name = Name.Replace("#ctor", baseName.Substring(baseName.LastIndexOf('.') + 1));

                IsConstructor = true;
            }

            var methodName = Name.Substring(0, Name.IndexOf('('));
            var paramList = Name.Replace(methodName, string.Empty);
            paramList = paramList.Substring(1, paramList.Length - 2);

            Name = $"{methodName}({string.Join(", ", paramList.Split(','))})";

            Params = GetParams(paramList.Split(','), xElement);
        }

        private IEnumerable<ParamDoc> GetParams(string[] parameterTypes, XElement xElement)
        {
            int paramIndex = 0;
            var parameterElements = from e in xElement.Elements()
                                    where e.Name == "param"
                                    select e;

            return (from p in parameterElements
                    select new ParamDoc(p, parameterTypes[paramIndex++])).ToArray();
        }
    }
}
