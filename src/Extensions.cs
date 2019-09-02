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
            var url = new UriBuilder(contentUrl);
            if (url.Query != null && url.Query.Length > 1)
                url.Query = url.Query.Substring(1) + "&" + version;
            else
                url.Query = version;

            return url.Uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.PathAndQuery, UriFormat.Unescaped);
        }

        public static string VersionedContent(this UrlHelper helper, string assetPath)
        {
            var fullPath = helper.RequestContext.HttpContext.Server.MapPath(assetPath);
            var version = AssetVersionCache.GetOrAddCachedVersion(fullPath, helper.RequestContext.HttpContext.Cache);

            if (version != null) { 
                // append hash to query string
                return Extensions.AppendVersion(helper.Content(assetPath), version);
            }

            return helper.Content(assetPath);
        }
    }
}
