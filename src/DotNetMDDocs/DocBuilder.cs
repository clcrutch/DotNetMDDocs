// <copyright file="DocBuilder.cs" company="Chris Crutchfield">
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

using System;
using DotNetMDDocs.Markdown;
using DotNetMDDocs.XmlDocParser;

namespace DotNetMDDocs
{
    public abstract class DocBuilder
    {
        public DocBuilder(TypeDoc type, BaseDoc baseDoc, Document document)
        {
            this.Type = type;
            this.BaseDoc = baseDoc;
            this.Document = document;
        }

        protected TypeDoc Type { get; private set; }

        protected BaseDoc BaseDoc { get; private set; }

        protected Document Document { get; private set; }

        public string Generate()
        {
            var md = new MDDocument();

            // Add the header.
            md.AddElement(new MDH1
            {
                Text = this.GetHeader()
            });

            if (!string.IsNullOrEmpty(this.BaseDoc.Summary))
            {
                md.AddElement(new MDQuote
                {
                    Quote = this.BaseDoc.Summary
                });
            }

            md.AddElement(new MDBold
            {
                Text = "Namespace:"
            });

            md.AddElement(new MDText
            {
                Text = $" {this.Type.Namespace}{Environment.NewLine}{Environment.NewLine}"
            });

            md.AddElement(new MDBold
            {
                Text = "Assembly:"
            });

            md.AddElement(new MDText
            {
                Text = $" {this.Document.Assembly.Name} (in {this.Document.Assembly.Name}.dll){Environment.NewLine}"
            });

            this.OnBeforeSyntax(md);

            md.AddElement(new MDH2
            {
                Text = "Syntax"
            });

            md.AddElement(new MDCode
            {
                Code = this.BaseDoc.CodeSyntax,
                Language = "csharp"
            });

            this.OnAfterSyntax(md);

            this.OnBeforeRemarks(md);

            if (!string.IsNullOrWhiteSpace(this.BaseDoc.Remarks))
            {
                md.AddElement(new MDH2
                {
                    Text = "Remarks"
                });

                md.AddElement(new MDText
                {
                    Text = this.BaseDoc.Remarks
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
