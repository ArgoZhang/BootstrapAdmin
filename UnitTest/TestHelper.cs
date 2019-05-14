using Longbow.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    public static class TestHelper
    {
        public static string SQLServerConnectionString { get; set; }

        public static string SQLiteConnectionString { get; set; }

        public static string MySqlConnectionString { get; set; }

        public static string NpgSqlConnectionString { get; set; }

        public static string MongoDBName { get; set; }

        /// <summary>
        /// 获得当前工程解决方案目录
        /// </summary>
        /// <returns></returns>
        public static string RetrieveSolutionPath()
        {
            var dirSeparator = Path.DirectorySeparatorChar;
            var paths = AppContext.BaseDirectory.SpanSplit($"{dirSeparator}.vs{dirSeparator}");
            return paths.Count > 1 ? paths[0] : Path.Combine(AppContext.BaseDirectory, $"..{dirSeparator}..{dirSeparator}..{dirSeparator}..{dirSeparator}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string RetrievePath(string folder)
        {
            var soluFolder = RetrieveSolutionPath();
            return Path.Combine(soluFolder, folder);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CopyLicense()
        {
            var licFile = RetrievePath($"Scripts{Path.DirectorySeparatorChar}Longbow.lic");

            var targetFile = Path.Combine(AppContext.BaseDirectory, "Longbow.lic");
            if (!File.Exists(targetFile))
            {
                File.Copy(licFile, targetFile, true);
            }
        }

        public static void ConfigureWebHost(IWebHostBuilder builder, DatabaseProviderType providerName = DatabaseProviderType.SqlServer)
        {
            if (providerName == DatabaseProviderType.SqlServer)
            {
                builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("ConnectionStrings:ba", SQLServerConnectionString),
                    new KeyValuePair<string, string>("DB:0:Enabled", "true")
                }));
            }

            if (providerName == DatabaseProviderType.SQLite)
            {
                var dbPath = RetrievePath($"UnitTest{Path.DirectorySeparatorChar}DB{Path.DirectorySeparatorChar}UnitTest.db");
                var dbFile = Path.Combine(AppContext.BaseDirectory, "UnitTest.db");
                File.Copy(dbPath, dbFile, true);

                builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:1:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:1:ConnectionStrings:ba", SQLiteConnectionString)
                }));
            }

            if (providerName == DatabaseProviderType.MySql)
            {
                builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:2:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:2:ConnectionStrings:ba", MySqlConnectionString)
                }));
            }

            if (providerName == DatabaseProviderType.Npgsql)
            {
                builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:2:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:3:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:3:ConnectionStrings:ba", NpgSqlConnectionString)
                }));
            }
        }
    }
}
