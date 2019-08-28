using System.Configuration;
using static DotnetConfig.Tests.ResourcePathHelper;
using NUnit.Framework;

namespace DotnetConfig.Tests
{
    [TestFixture]
    public class WebConfigTests
    {
        [Test]
        public void OverrideEmptySections()
        {
            var config = GetPath("web_empty_sections.config");
            var team = ConfigurationManager.AppSettings["Team"];
            var @default = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            WebConfig.Override(config);
            
            Assert.AreEqual(team, ConfigurationManager.AppSettings["Team"]);
            Assert.AreEqual(@default, ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }

        [Test]
        public void OverrideWithValues()
        {
            var config = GetPath("web_with_values.config");
            var team = ConfigurationManager.AppSettings["Team"];
            var @default = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            WebConfig.Override(config);
            
            Assert.AreNotEqual(team, ConfigurationManager.AppSettings["Team"]);
            Assert.AreNotEqual(@default, ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            Assert.AreEqual("B", ConfigurationManager.AppSettings["Team"]);
            StringAssert.Contains("DB_B", ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }
    }
}
