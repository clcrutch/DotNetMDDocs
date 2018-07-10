using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs
{
    public abstract class DocBuilder
    {
        protected readonly TypeDoc type;
        protected readonly Document document;

        public DocBuilder(TypeDoc type, Document document)
        {
            this.type = type;
            this.document = document;
        }

        public string Generate()
        {
            var md = new MDDocument();

            // Add the header.
            md.AddElement(new MDH1
            {
                Text = GetHeader()
            });

            if (!string.IsNullOrEmpty(type.Summary))
            {
                md.AddElement(new MDQuote
                {
                    Quote = type.Summary
                });
            }

            md.AddElement(new MDBold
            {
                Text = "Namespace:"
            });

            md.AddElement(new MDText
            {
                Text = $" {type.Namespace}{Environment.NewLine}"
            });

            md.AddElement(new MDBold
            {
                Text = "Assembly:"
            });

            md.AddElement(new MDText
            {
                Text = $" {document.Assembly.Name} (in {document.Assembly.Name}.dll){Environment.NewLine}"
            });

            md.AddElement(new MDH2
            {
                Text = "Syntax"
            });

            OnSyntaxGenerate(md);

            if (!string.IsNullOrWhiteSpace(type.Remarks))
            {
                md.AddElement(new MDH2
                {
                    Text = "Remarks"
                });

                md.AddElement(new MDText
                {
                    Text = type.Remarks
                });
            }

            OnGenerate(md);

            return md.Generate();
        }

        protected abstract string GetHeader();

        protected virtual void OnGenerate(MDDocument md)
        {

        }

        protected virtual void OnSyntaxGenerate(MDDocument md)
        {

        }
    }
}
