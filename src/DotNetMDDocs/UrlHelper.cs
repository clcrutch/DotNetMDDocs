using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMDDocs
{
    public static class UrlHelper
    {
        private static Dictionary<string, string> urlMap;

        static UrlHelper()
        {
            urlMap = new Dictionary<string, string>();
        }

        public static void AddType(string type, string url)
        {
            urlMap.Add(type, url);
        }

        public static string GetType(string type)
        {
            if (urlMap.TryGetValue(type, out string url))
            {
                return url;
            }

            return $"https://www.google.com/search?q={type}&btnI=";
        }
    }
}
