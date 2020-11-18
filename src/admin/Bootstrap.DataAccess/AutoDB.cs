using PetaPoco;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 数据库自动生成实体类
    /// </summary>
    public class AutoDB
    {
        /// <summary>
        /// 数据库检查方法
        /// </summary>
        public virtual void EnsureCreated(string folder)
        {
            using var db = Longbow.Data.DbManager.Create();
            db.CommandTimeout = 5000;
            switch (db.Provider.GetType().Name)
            {
                case "SQLiteDatabaseProvider":
                    if (db.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Users'") == 0) GenerateSQLiteDB(db, folder);
                    break;
                case "SqlServerDatabaseProvider":
                    using (var newDB = ModifyConnectionString(db))
                    {
                        if (newDB.ExecuteScalar<int?>("select COUNT(*) from sys.databases where name = N'BootstrapAdmin'") == 0) GenerateDB(folder);
                    }
                    break;
                case "MySqlDatabaseProvider":
                case "MariaDbDatabaseProvider":
                    if (db.ExecuteScalar<int>("select count(*) from information_schema.tables where table_name ='Users' and Table_Schema = 'BootstrapAdmin'") == 0) GenerateDB(folder);
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
            return new Database(string.Join(";", newsegs), provider);
        }

        private void GenerateSQLiteDB(IDatabase db, string folder)
        {
            var initFile = Path.Combine(folder, "Install.sql");
            if (File.Exists(initFile))
            {
                var sql = File.ReadAllText(initFile);
                db.Execute(sql);

                initFile = Path.Combine(folder, "InitData.sql");
                if (File.Exists(initFile))
                {
                    sql = File.ReadAllText(initFile);
                    db.Execute(sql);
                }
            }
        }

        /// <summary>
        /// 执行建库脚本
        /// </summary>
        protected void GenerateDB(string folder)
        {
            // 检查 install.ps1 脚本
            var file = Path.Combine(folder, $"install.ps1");
            if (File.Exists(file))
            {
                var psi = new ProcessStartInfo("powershell", $"{file} \"{folder}\"");
                var p = Process.Start(psi);
                p!.WaitForExit();
            }
        }
    }
}
