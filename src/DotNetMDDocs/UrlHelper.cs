// <copyright file="UrlHelper.cs" company="Chris Crutchfield">
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

namespace DotNetMDDocs
{
    /// <summary>
    /// Helper for converting types into URLs.
    /// </summary>
    public static class UrlHelper
    {
        private static Dictionary<string, string> urlMap;

        static UrlHelper()
        {
            urlMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds a type to the URL helper to recall later.
        /// </summary>
        /// <param name="type">The type to add.</param>
        /// <param name="url">The URL that matches the type.</param>
        public static void AddType(string type, string url)
        {
            urlMap.Add(type, url);
        }

        /// <summary>
        /// Gets the URL for the type.
        /// </summary>
        /// <param name="type">The type to get the Url for.</param>
        /// <returns>Either the Url previously inserted, or the Google I'm feeling lucky results if unknown.</returns>
        public static string GetUrl(string type)
        {
            if (urlMap.TryGetValue(type, out string url))
            {
                return url;
            }

            return $"https://www.google.com/search?q={type}&btnI=";
        }
    }
}
