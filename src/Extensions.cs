using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace System.Web.Mvc.AssetVersioning
{
    public static class UrlHelpers
    {
        private const string CACHE_KEY = "__AssetVersions__";

        private static ConcurrentDictionary<string, string> GetOrCreateAssetCache(HttpContextBase context)
        {
            var cache = context.Cache[CACHE_KEY] as ConcurrentDictionary<string, string>;

            if (cache == null)
            {
                cache = new ConcurrentDictionary<string, string>();
                context.Cache[CACHE_KEY] = cache;
            }

            return cache;
        }

        public static string VersionedContent(this UrlHelper helper, string assetPath)
        {
            // resolve file
            var fullPath = helper.RequestContext.HttpContext.Server.MapPath(assetPath);

            if (File.Exists(fullPath))
            {
                var cache = GetOrCreateAssetCache(helper.RequestContext.HttpContext);
                var version = string.Empty;

                if (cache.ContainsKey(fullPath))
                {
                    version = cache[fullPath];
                }
                else
                {
                    using (var stream = File.OpenRead(fullPath))
                    {                       
                        using (var sha256 = SHA256.Create())
                        {
                            version = Convert.ToBase64String(sha256.ComputeHash(stream));
                        }
                    }

                    version = cache.GetOrAdd(fullPath, version);
                }

                // append hash to query string
                var url = new UriBuilder(helper.Content(assetPath));
                if (url.Query != null && url.Query.Length > 1)
                    url.Query = url.Query.Substring(1) + "&" + version;
                else
                    url.Query = version;

                return url.ToString();
            }

            return helper.Content(assetPath);
        }
    }
}
