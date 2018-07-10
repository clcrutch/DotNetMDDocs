using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDLink : IMDElement
    {
        public string Text { get; set; }
        public string Url { get; set; }

        public string Generate()
        {
            return $"[{Text}]({Url})";
        }
    }
}
