using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDCode : IMDElement
    {
        public string Code { get; set; }
        public string Language { get; set; }

        public string Generate()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"~~~~{Language}");
            stringBuilder.AppendLine(Code);
            stringBuilder.AppendLine("~~~~");

            return stringBuilder.ToString();
        }
    }
}
