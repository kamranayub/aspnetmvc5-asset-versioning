using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace System.Web.Mvc.AssetVersioning
{
    public static class AssetVersionCache
    {
        private const string CACHE_KEY = "__AssetVersions__";

        private static ConcurrentDictionary<string, string> GetOrCreateAssetCache(Cache cache)
        {
            var assetCache = cache[CACHE_KEY] as ConcurrentDictionary<string, string>;

            if (assetCache == null)
            {
                assetCache = new ConcurrentDictionary<string, string>();
                cache[CACHE_KEY] = assetCache;
            }

            return assetCache;
        }

        public static string ComputeHash(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                using (var sha256 = SHA256.Create())
                {
                    return Convert.ToBase64String(sha256.ComputeHash(stream));
                }
            }
        }

        public static string GetOrAddCachedVersion(string filePath, Cache cache)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            var assetCache = GetOrCreateAssetCache(cache);
            var version = string.Empty;

            if (assetCache.ContainsKey(filePath))
            {
                version = assetCache[filePath];
            }
            else
            {
                version = AssetVersionCache.ComputeHash(filePath);
                version = assetCache.GetOrAdd(filePath, version);
            }

            return version;
        }
    }
}