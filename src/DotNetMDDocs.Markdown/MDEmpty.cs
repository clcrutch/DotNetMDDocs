using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDEmpty : IMDElement
    {
        public static MDEmpty Empty { get; } = new MDEmpty();


        private MDEmpty()
        {
        }

        public string Generate()
        {
            return string.Empty;
        }
    }
}
