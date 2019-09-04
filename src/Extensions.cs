using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace System.Web.Mvc.AssetVersioning
{
    public static class Extensions
    {
        public static string AppendVersion(string contentUrl, string version)
        {
            if (contentUrl.Contains("?"))
            {
                return contentUrl + "&" + version;
            }
            else
            {
                return contentUrl + "?" + version;
            }
        }

        public static string VersionedContent(this UrlHelper helper, string contentPath)
        {
            var fullPath = helper.RequestContext.HttpContext.Server.MapPath(contentPath);
            var version = AssetVersionCache.GetOrAddCachedVersion(fullPath, helper.RequestContext.HttpContext.Cache);

            if (version != null)
            {
                var relativePath = helper.Content(contentPath);
                // append hash to query string
                return AppendVersion(relativePath, version);
            }

            return helper.Content(contentPath);
        }
    }
}
