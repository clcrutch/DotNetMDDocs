// <copyright file="IEnumerableICommentBlockElementExtensions.cs" company="Chris Crutchfield">
// Copyright (C) 2017  Chris Crutchfield
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see &lt;http://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using DotNetDocs.CommentBlockElements;
using DotNetMDDocs.Markdown;

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
                if (item is StringCommentBlockElement stringComment)
                {
                    markdownGroup.AddElement(new MDText
                    {
                        Text = stringComment.Content.Trim(),
                    });
                }
                else if (item is SeeCommentBlockElement see)
                {
                    markdownGroup.AddElement(new MDLink
                    {
                        Text = see.TypeName,
                        Url = UrlHelper.GetUrl(see.TypeName),
                    });
                }
                else if (item is ParamRefCommentBlockElement paramRef)
                {
                    markdownGroup.AddElement(new MDText
                    {
                        Text = paramRef.ParameterName,
                    });
                }
            }

            return markdownGroup;
        }
    }
}
