using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public abstract class MDHeader : IMDElement
    {
        private readonly int level;

        public string Text { get; set; }

        public MDHeader(int level)
        {
            this.level = level;
        }

        public string Generate()
        {
            return $"{new String('#', level)} {Text}{Environment.NewLine}";
        }
    }
}
