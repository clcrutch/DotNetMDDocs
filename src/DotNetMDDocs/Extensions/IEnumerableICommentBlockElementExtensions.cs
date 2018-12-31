using DotNetDocs.CommentBlockElements;
using DotNetMDDocs.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMDDocs.Extensions
{
    public static class IEnumerableICommentBlockElementExtensions
    {
        public static MDGroup ConvertToMDGroup(this IEnumerable<ICommentBlockElement> @this)
        {
            if (@this == null || !@this.Any())
            {
                return null;
            }

            var markdownGroup = new MDGroup();

            foreach (var item in @this)
            {
                if (item is StringCommentBlockElement)
                {
                    markdownGroup.AddElement(new MDText
                    {
                        Text = ((StringCommentBlockElement)item).Content,
                    });
                }
                else if (item is SeeCommentBlockElement)
                {
                    var see = item as SeeCommentBlockElement;

                    markdownGroup.AddElement(new MDLink
                    {
                        Text = see.TypeName,
                        Url = UrlHelper.GetType(see.TypeName),
                    });
                }
            }

            return markdownGroup;
        }
    }
}
