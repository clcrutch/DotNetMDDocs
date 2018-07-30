// <copyright file="MethodDocBuilder.cs" company="Chris Crutchfield">
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
using DotNetDocs;
using DotNetMDDocs.Markdown;

namespace DotNetMDDocs
{
    public class MethodDocBuilder : DocBuilder
    {
        public const string SPACE = "&nbsp;";

        public MethodDocBuilder(MethodDocumentation methodDocumentation, TypeDocumentation typeDocumentation, AssemblyDocumentation assemblyDocumentation)
             : base(methodDocumentation, typeDocumentation, assemblyDocumentation)
        {
        }

        private MethodDocumentation MethodDocumentation => (MethodDocumentation)this.Documentation;

        protected override string GetHeader()
        {
            return $"{this.TypeDocumentation.Name}.{this.MethodDocumentation.Name} {(this.MethodDocumentation.IsConstructor ? "Constructor" : "Method")}";
        }

        protected override void OnAfterSyntax(MDDocument md)
        {
            md.AddElement(new MDH5
            {
                Text = "Parameters",
            });

            /*foreach (var param in this.method.Params)
            {
                md.AddElement(new MDItalics
                {
                    Text = param.Name
                });
                md.AddElement(new MDText
                {
                    Text = $"{Environment.NewLine}{Environment.NewLine}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}"
                });
                md.AddElement(new MDText
                {
                    Text = $"Type: {param.Type}{Environment.NewLine}{Environment.NewLine}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}{SPACE}"
                });
                md.AddElement(new MDText
                {
                    Text = $"{param.Summary}{Environment.NewLine}{Environment.NewLine}"
                });
                md.AddElement(new MDText
                {
                    Text = $"{Environment.NewLine}"
                });
            }*/
        }
    }
}
