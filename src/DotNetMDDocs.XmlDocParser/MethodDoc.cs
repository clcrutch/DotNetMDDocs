// <copyright file="MethodDoc.cs" company="Chris Crutchfield">
// Copyright (C) 2017  Chris Crutchfield
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see &lt;http://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class MethodDoc : BaseDoc
    {
        public MethodDoc(XElement xElement, string baseName)
            : base("M", xElement, $"{baseName}.")
        {
            if (!this.Name.EndsWith(")"))
            {
                this.Name = $"{this.Name}()";
            }

            if (this.Name.Contains("#ctor"))
            {
                this.Name = this.Name.Replace("#ctor", baseName.Substring(baseName.LastIndexOf('.') + 1));

                this.IsConstructor = true;
            }

            var methodName = this.Name.Substring(0, this.Name.IndexOf('('));
            var paramList = this.Name.Replace(methodName, string.Empty);
            paramList = paramList.Substring(1, paramList.Length - 2);

            this.Name = $"{methodName}({string.Join(", ", paramList.Split(','))})";

            this.Params = this.GetParams(paramList.Split(','), xElement);
        }

        public bool IsConstructor { get; private set; }

        public IEnumerable<ParamDoc> Params { get; private set; }

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
