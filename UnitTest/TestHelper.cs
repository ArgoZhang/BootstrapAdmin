using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    public static class TestHelper
    {
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

        private const string SqlConnectionString = "Data Source=.;Initial Catalog=UnitTest;User ID=sa;Password=sa";
        private const string SQLiteConnectionString = "Data Source=UnitTest.db;";
        private const string MySqlConnectionString = "Server=localhost;Database=UnitTest;Uid=argozhang;Pwd=argo@163.com;SslMode=none;allowPublicKeyRetrieval=true";
        private const string NpgSqlConnectionString = "Server=localhost;Database=UnitTest;User ID=argozhang;Password=sa;";

        public static void ConfigureWebHost(IWebHostBuilder builder, string providerName = Longbow.Data.DatabaseProviderType.SqlServer)
        {
            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("ConnectionStrings:ba", SqlConnectionString),
                new KeyValuePair<string, string>("DB:0:Enabled", "true")
            }));

            if (providerName == Longbow.Data.DatabaseProviderType.SQLite)
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

            if (providerName == Longbow.Data.DatabaseProviderType.MySql)
            {
                builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                    new KeyValuePair<string, string>("DB:2:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:2:ConnectionStrings:ba", MySqlConnectionString)
                }));
            }

            if (providerName == Longbow.Data.DatabaseProviderType.Npgsql)
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
