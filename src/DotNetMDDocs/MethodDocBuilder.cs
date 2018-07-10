using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs
{
    public class MethodDocBuilder : DocBuilder
    {
        private readonly MethodDoc method;

        public MethodDocBuilder(MethodDoc method, TypeDoc type, Document document)
             : base(type, document)
        {
            this.method = method;
        }

        protected override string GetHeader()
        {
            return $"{type.Name}.{method.Name} {(method.IsConstructor ? "Constructor" : "Method")}";
        }

        protected override void OnAfterSyntax(MDDocument md)
        {
            md.AddElement(new MDH5
            {
                Text = "Parameters"
            });

            foreach (var param in method.Params)
            {
                md.AddElement(new MDItalics
                {
                    Text = param.Name
                });
                md.AddElement(new MDText
                {
                    Text = $"{Environment.NewLine}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                });
                md.AddElement(new MDText
                {
                    Text = $"Type: {param.Type}{Environment.NewLine}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                });
                md.AddElement(new MDText
                {
                    Text = $"{param.Summary}{Environment.NewLine}"
                });
                md.AddElement(new MDText
                {
                    Text = $"{Environment.NewLine}"
                });
            }
        }
    }
}
