using DotNetDocs.CommentBlockElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetDocs
{
    public static class IEnumerableICommentBlockElementExtensions
    {
        public static string ConvertToString(this IEnumerable<ICommentBlockElement> @this)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in @this)
            {
                if (item is StringCommentBlockElement)
                {
                    stringBuilder.Append(((StringCommentBlockElement)item).Content);
                }
                else if (item is SeeCommentBlockElement)
                {
                    var see = item as SeeCommentBlockElement;
                    stringBuilder.Append(see.TypeName);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
