// <copyright file="IObjectDocumentation.cs" company="Chris Crutchfield">
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
using DotNetDocs.CommentBlockElements;
using DotNetDocs.ObjectDocumentations;

namespace DotNetDocs.Mixins.Contracts
{
    /// <summary>
    /// Represents an object documentation such as a type or member.
    /// </summary>
    public interface IObjectDocumentation : IDocumentation
    {
        /// <summary>
        /// Gets the code declaration for the current member.
        /// </summary>
        string Declaration { get; }

        /// <summary>
        /// Gets the <see cref="TypeDocumentation"/> which contains the current documentation.
        /// </summary>
        TypeDocumentation DeclaringType { get; }

        /// <summary>
        /// Gets the remarks for the current member.
        /// </summary>
        IEnumerable<ICommentBlockElement> Remarks { get; }

        /// <summary>
        /// Gets the summary from the XmlElement.
        /// </summary>
        IEnumerable<ICommentBlockElement> Summary { get; }
    }
}
