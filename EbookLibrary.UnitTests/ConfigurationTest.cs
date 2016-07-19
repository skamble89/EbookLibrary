using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using global_config = System.Configuration;
using lib_config = EbookLibrary.Configuration;

namespace EbookLibrary.UnitTests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void ConfigSectionLoadingTest()
        {
            lib_config.LibrarySection config = global_config.ConfigurationManager.GetSection("ebooklibrary.config") as lib_config.LibrarySection;
            Assert.AreEqual(config.Libraries.Count, 1, "1 config element found");
        }

        [TestMethod]
        public void ConfigSectionNameMatchTest()
        {
            lib_config.LibrarySection config = global_config.ConfigurationManager.GetSection("ebooklibrary.config") as lib_config.LibrarySection;
            Assert.AreEqual(config.Libraries[0].Name, "Gutenberg", "Name does not match the one defined in config");
        }

        [TestMethod]
        public void ConfigSectionTypeMatchTest()
        {
            lib_config.LibrarySection config = global_config.ConfigurationManager.GetSection("ebooklibrary.config") as lib_config.LibrarySection;
            Assert.AreEqual(config.Libraries[0].Type, "EbookLibrary.Gutenberg.Library, EbookLibrary.Gutenberg", "Type does not match the one defined in config");
        }

        [TestMethod]
        public void LibraryFetchByNameTest()
        {
            lib_config.LibrarySection config = global_config.ConfigurationManager.GetSection("ebooklibrary.config") as lib_config.LibrarySection;
            Assert.IsTrue(config.Libraries["Gutenberg"] != null, "Unable to fetch library by name");
        }
    }
}
