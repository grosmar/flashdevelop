using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public void TestAdvancedSearchMatch()
        {
            //var list = new List<string> { "item1", "item2" };
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "sc", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "Sc", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "sC", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "SC", "/"));

            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "somec", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "omec", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("SomeClass", "omeclas", "/"));

            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "alc", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "advlc", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "aderClass", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "aderClass", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dvancedlc", "/"));
            Assert.IsTrue(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dlc", "/"));

            Assert.IsFalse(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "ar", "/"));
            Assert.IsFalse(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dvancedlr", "/"));
            Assert.IsFalse(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "dvanclc", "/"));
            Assert.IsFalse(ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "adass", "/"));
        }

        [Test]
        public void TestAdvancedSearchMatchSpeed()
        {

            for (int i = 0; i < 10000; i++ )
            {
                ProjectManager.Helpers.SearchUtil.AdvancedSearchMatch("AdvancedLoaderClass", "advancloadeclas", "/");
            }
            
        }
    }
}
