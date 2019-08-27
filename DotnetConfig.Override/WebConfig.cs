using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace DotnetConfig.Override
{
    public static class WebConfig
    {
        public static void Override(string suffix = "Override")
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var configOverride = HttpContext.Current.Server.MapPath($"~/Web.{suffix}.config");
            if (!File.Exists(configOverride))
            {
                return;
            }

            ReplaceConfig(configOverride);
        }

        private static void ReplaceConfig(string webConfig)
        {
            var @override = XDocument.Parse(File.ReadAllText(webConfig));

            // Load allowed override configs
            var settings = GetAppSettings(@override);
            var connections = GetConnectionString(@override);

            // Override configs if available.
            if (settings.Count > 0) ReplaceSettings(settings);
            if (connections.Count > 0) ReplaceConnections(connections);
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
                .ToDictionary(s => s.Attribute("key").Value, s => s.Attribute("value").Value);

        private static Dictionary<string, string> GetConnectionString(XDocument doc) => doc
                .Elements("configuration")
                .Elements("connectionStrings")
                .Elements("add")
                .ToDictionary(s => s.Attribute("name").Value, s => s.Attribute("connectionString").Value);
    }
}
