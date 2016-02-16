using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectManager.Helpers;

namespace ProjectManager.Tests
{
    [TestFixture]
    public class SearchUtilTests
    {
        [TestFixtureSetUp]
        public void initialize()
        {
         
            
        }

        [Test]
        public void TestGetMatchedItems()
        {
            List<String> source = new List<string> { "AppletVar", "ApprovedProperty", "SomeClass", "SomeAdvancedClass", "OtherStuff", "ThatText" };

            CollectionAssert.AreEqual( new List<string> { "SomeClass", "SomeAdvancedClass" }, SearchUtil.GetMatchedItems(source, "sc", "", 100) );
            CollectionAssert.AreEqual(new List<string> { "SomeClass", "SomeAdvancedClass" }, SearchUtil.GetMatchedItems(source, "sC", "", 100));
            CollectionAssert.AreEqual(new List<string> { "ApprovedProperty", "AppletVar" }, SearchUtil.GetMatchedItems(source, "AP", "", 100));
        }

        [Test]
        public void TestAdvancedSearchMatch()
        {
            //var list = new List<string> { "item1", "item2" };
            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("SomeClass", "sc", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("SomeClass", "Sc", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("SomeClass", "sC", "/"));
            Assert.AreEqual(2, SearchUtil.AdvancedSearchMatch("SomeClass", "SC", "/"));

            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("SomeClass", "somec", "/"));
            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("SomeClass", "omec", "/"));
            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("SomeClass", "omeclas", "/"));

            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("AppletProperty", "ap", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("AppletProperty", "aP", "/"));
            Assert.AreEqual(2, SearchUtil.AdvancedSearchMatch("AppletProperty", "AP", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("AppletProperty", "Ap", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("AppletProperty", "App", "/"));

            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "alC", "/"));
            Assert.AreEqual(2, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "aLC", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "aLc", "/"));
            Assert.AreEqual(2, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "AlC", "/"));
            Assert.AreEqual(2, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "ALc", "/"));
            Assert.AreEqual(3, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "ALC", "/"));
            Assert.AreEqual(2, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "ACL", "/"));

            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "alc", "/"));
            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "advlc", "/"));
            Assert.AreEqual(1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "aderClass", "/"));
            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dvancedlc", "/"));
            Assert.AreEqual(0, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dlc", "/"));

            Assert.AreEqual(-1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "ar", "/"));
            Assert.AreEqual(-1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dvancedlr", "/"));
            Assert.AreEqual(-1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dvanclc", "/"));
            Assert.AreEqual(-1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "adass", "/"));
            Assert.AreEqual(-1, SearchUtil.AdvancedSearchMatch("AdvancedLoaderTest", "ATL", "/"));

        }

        [Test]
        public void TestAdvancedSearchMatchSpeed()
        {

            for (int i = 0; i < 10000; i++ )
            {
                SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "advancloadeclas", "/");
            }
            
        }
    }
}
