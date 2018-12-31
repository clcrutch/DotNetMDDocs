﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs.Markdown
{
    public class MDGroup : IMDElement
    {
        private readonly List<IMDElement> elements = new List<IMDElement>();

        public void AddElement(IMDElement element)
        {
            this.elements.Add(element);
        }

        public string Generate()
        {
            var stringBuilder = new StringBuilder();

            foreach (var element in this.elements)
            {
                stringBuilder.Append(element.Generate());
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
