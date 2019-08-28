using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace DotnetConfig.Override
{
    public static class WebConfig
    {
        private const string PathOverride = "~/Web.Override.config";

        public static void Override()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var configOverride = HttpContext.Current.Server.MapPath(PathOverride);
            if (!File.Exists(configOverride))
            {
                return;
            }

            Override(configOverride);
        }

        private static void Override(string pathOverride)
        {
            var @override = XDocument.Parse(File.ReadAllText(pathOverride));

            // Load allowed override configs
            var settings = GetAppSettings(@override);
            var connections = GetConnectionString(@override);

            // Override configs if available.
            ReplaceSettings(settings);
            ReplaceConnections(connections);
        }

        private static void ReplaceSettings(Dictionary<string, string> values)
        {
            foreach (var item in values)
            {
                ConfigurationManager.AppSettings[item.Key] = item.Value;
            }
        }
        
        private static void ReplaceConnections(Dictionary<string, string> values)
        {
            var field = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null) return;

            foreach (var item in values)
            {
                var connection = ConfigurationManager.ConnectionStrings[item.Key];
                if (connection == null) continue;

                field.SetValue(connection, false);
                connection.ConnectionString = item.Value;
                field.SetValue(connection, true);
            }
        }

        private static Dictionary<string, string> GetAppSettings(XDocument doc) => doc
                .Elements("configuration")
                .Elements("appSettings")
                .Elements("add")
                .ToDictionary(s => s.Attribute("key")?.Value, s => s.Attribute("value")?.Value);

        private static Dictionary<string, string> GetConnectionString(XDocument doc) => doc
                .Elements("configuration")
                .Elements("connectionStrings")
                .Elements("add")
                .ToDictionary(s => s.Attribute("name")?.Value, s => s.Attribute("connectionString")?.Value);
    }
}
