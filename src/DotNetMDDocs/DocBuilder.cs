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
        protected readonly BaseDoc baseDoc;
        protected readonly Document document;

        public DocBuilder(TypeDoc type, BaseDoc baseDoc, Document document)
        {
            this.type = type;
            this.baseDoc = baseDoc;
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

            if (!string.IsNullOrEmpty(baseDoc.Summary))
            {
                md.AddElement(new MDQuote
                {
                    Quote = baseDoc.Summary
                });
            }

            md.AddElement(new MDBold
            {
                Text = "Namespace:"
            });

            md.AddElement(new MDText
            {
                Text = $" {type.Namespace}{Environment.NewLine}{Environment.NewLine}"
            });

            md.AddElement(new MDBold
            {
                Text = "Assembly:"
            });

            md.AddElement(new MDText
            {
                Text = $" {document.Assembly.Name} (in {document.Assembly.Name}.dll){Environment.NewLine}"
            });

            OnBeforeSyntax(md);

            md.AddElement(new MDH2
            {
                Text = "Syntax"
            });

            md.AddElement(new MDCode
            {
                Code = baseDoc.CodeSyntax,
                Language = "csharp"
            });

            OnAfterSyntax(md);

            OnBeforeRemarks(md);

            if (!string.IsNullOrWhiteSpace(baseDoc.Remarks))
            {
                md.AddElement(new MDH2
                {
                    Text = "Remarks"
                });

                md.AddElement(new MDText
                {
                    Text = baseDoc.Remarks
                });
            }

            return md.Generate();
        }

        protected abstract string GetHeader();

        protected virtual void OnAfterSyntax(MDDocument md)
        {

        }

        protected virtual void OnBeforeRemarks(MDDocument md)
        {

        }

        protected virtual void OnBeforeSyntax(MDDocument md)
        {

        }
    }
}
