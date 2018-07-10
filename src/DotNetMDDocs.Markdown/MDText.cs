using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDText : IMDElement
    {
        public string Text { get; set; }

        public string Generate()
        {
            return Text;
        }
    }
}
