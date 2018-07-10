using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetMDDocs.XmlDocParser
{
    public class TypeDoc : BaseDoc
    {
        public string Namespace { get; set; }
        public IEnumerable<MethodDoc> Constructors { get; private set; }
        public IEnumerable<PropertyDoc> Properties { get; private set; }
        public IEnumerable<MethodDoc> Methods { get; private set; }
        public IEnumerable<FieldDoc> Fields { get; private set; }

        public string FullName => $"{Namespace}.{Name}";

        public TypeDoc(XElement xElement, XDocument xDocument)
            : base("T", xElement, string.Empty)
        {
            Namespace = xElement.Attribute("name").Value.Substring("T:".Length);
            Namespace = Namespace.Substring(0, Namespace.LastIndexOf('.'));

            Name = Name.Replace($"{Namespace}.", string.Empty);

            Constructors = GetConstructors(xDocument);
            Properties = GetProperties(xDocument);
            Methods = GetMethods(xDocument);
            Fields = GetFields(xDocument);
        }

        private IEnumerable<MethodDoc> GetConstructors(XDocument xDocument)
        {
            return (from m in GetMembers("M", xDocument)
                    where m.Attribute("name").Value.Contains("#ctor")
                    select new MethodDoc(m, FullName)).ToArray();
        }

        private IEnumerable<PropertyDoc> GetProperties(XDocument xDocument)
        {
            return (from m in GetMembers("P", xDocument)
                    select new PropertyDoc(m, FullName)).ToArray();
        }

        private IEnumerable<MethodDoc> GetMethods(XDocument xDocument)
        {
            return (from m in GetMembers("M", xDocument)
                    where !m.Attribute("name").Value.Contains("#ctor")
                    select new MethodDoc(m, FullName)).ToArray();
        }

        private IEnumerable<FieldDoc> GetFields(XDocument xDocument)
        {
            return (from m in GetMembers("F", xDocument)
                    select new FieldDoc(m, FullName)).ToArray();
        }

        private IEnumerable<XElement> GetMembers(string identifier, XDocument xDocument)
        {
            var members = (from e in xDocument.Root.Elements()
                           where e.Name == "members"
                           select e).Single();

            return from e in members.Elements()
                   where e.Name == "member" && e.Attribute("name").Value.StartsWith($"{identifier}:{FullName}.")
                   select e;
        }
    }
}
