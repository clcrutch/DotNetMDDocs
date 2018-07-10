using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDTableRow
    {
        public List<IMDElement> Cells { get; } = new List<IMDElement>();
    }
}
