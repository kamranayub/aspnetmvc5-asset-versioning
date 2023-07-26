using System;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.AssetVersioning;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AspNet.MVC5.AssetVersioning.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void AppendVersion_Should_Append_To_Empty_Url()
        {
            
            var path = Extensions.AppendVersion("/test.js", "123");
            
            Assert.AreEqual("/test.js?123", path);
        }

        [TestMethod]
        public void AppendVersion_Should_Append_To_Existing_Url()
        {

            var path = Extensions.AppendVersion("/test.js?h=foo", "123");

            Assert.AreEqual("/test.js?h=foo&123", path);
        }

        [TestMethod]
        public void ComputeHash_Returns_Sha256_Hash_of_File_Contents()
        {
            var hash = AssetVersionCache.ComputeHash("test.txt");

            Assert.AreEqual("e5bf55aca46b80b976cf055d50ec21c22859d9cc2561d8d04ea804555f86eb6e", hash);
        }

        [TestMethod]
        public void GetOrAddCachedVersion_Caches_File_Hash()
        {
            var cache = new System.Web.Caching.Cache();
            var hash = AssetVersionCache.GetOrAddCachedVersion("test.txt", cache);

            var assetCache = cache.Get("__AssetVersions__") as ConcurrentDictionary<string, string>;

            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, assetCache.Count);
            Assert.AreEqual("e5bf55aca46b80b976cf055d50ec21c22859d9cc2561d8d04ea804555f86eb6e", assetCache["test.txt"]);
            Assert.AreEqual(hash, assetCache["test.txt"]);

            var cachedHash = AssetVersionCache.GetOrAddCachedVersion("test.txt", cache);

            Assert.AreEqual(1, assetCache.Count);
            Assert.AreEqual(hash, cachedHash);
        }
    }
}
