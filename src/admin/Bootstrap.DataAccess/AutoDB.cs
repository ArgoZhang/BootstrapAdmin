using Longbow.Configuration;
using Microsoft.Extensions.Configuration;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 数据库自动生成实体类
    /// </summary>
    public class AutoDB
    {
        private static bool _init = false;
        private static object _locker = new object();

        /// <summary>
        /// 数据库检查方法
        /// </summary>
        public void CheckDB()
        {
            if (_init) return;

            // 阻止所有线程继续往下运行，等待数据库检查
            lock (_locker)
            {
                if (_init) return;

                // 数据检查
                Check();
            }
        }

        private void Check()
        {
            var dbSection = ConfigurationManager.GetSection("DB").GetChildren().FirstOrDefault(c => c.GetValue("Enabled", false));
            if (dbSection == null)
            {
                _init = true;
                return;
            }
            var folder = dbSection["SqlFolder"];
            if (folder.IsNullOrEmpty())
            {
                _init = true;
                return;
            }

            var db = Longbow.Data.DbManager.Create();
            CheckDbExists(db, folder);
            _init = true;
        }

        private void CheckDbExists(IDatabase db, string folder)
        {
            db.CommandTimeout = 5000;
            switch (db.Provider.GetType().Name)
            {
                case "SQLiteDatabaseProvider":
                    if (db.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Users'") == 0) GenerateSQLiteDB(db, folder);
                    break;
                case "SqlServerDatabaseProvider":
                    var newDB = ModifyConnectionString(db);
                    if (newDB.ExecuteScalar<int?>("select COUNT(1) from sys.databases where name = N'BootstrapAdmin'") == 0) GenerateSqlServer(folder);
                    break;
                case "MySqlDatabaseProvider":
                case "MariaDbDatabaseProvider":
                    if (db.ExecuteScalar<int>("select count(*) from information_schema.tables where table_name ='Users' and Table_Schema = 'BootstrapAdmin'") == 0) GenerateMySql(folder);
                    break;
            }
        }

        private IDatabase ModifyConnectionString(IDatabase db)
        {
            var conn = db.ConnectionString;
            var newsegs = new List<string>();
            var segs = conn.SpanSplit(";");
            segs.ForEach(s =>
            {
                if (s.StartsWith("Initial Catalog", StringComparison.OrdinalIgnoreCase)) newsegs.Add("Initial Catalog=master");
                else newsegs.Add(s);
            });
            var provider = db.Provider;
            db.Dispose();
            return new Database(string.Join(";", newsegs), provider);
        }

        private void GenerateSQLiteDB(IDatabase db, string folder)
        {
            var initFile = Path.Combine(folder, "Install.sql");
            var sql = File.ReadAllText(initFile);
            db.Execute(sql);

            initFile = Path.Combine(folder, "InitData.sql");
            sql = File.ReadAllText(initFile);
            db.Execute(sql);
        }

        private void GenerateSqlServer(string folder)
        {
            var psi = new ProcessStartInfo("powershell", Path.Combine(folder, $"install.ps1 \"{folder}\""));
            var p = Process.Start(psi);
            p.WaitForExit();
        }

        private void GenerateMySql(string folder)
        {
            // 没有环境暂时未写代码
        }
    }
}
