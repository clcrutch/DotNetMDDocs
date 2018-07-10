using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDQuote : IMDElement
    {
        public string Quote { get; set; }

        public string Generate()
        {
            return $"> {Quote}{Environment.NewLine}{Environment.NewLine}";
        }
    }
}
