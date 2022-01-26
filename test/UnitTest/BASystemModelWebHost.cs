// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.Threading.Tasks;
using Xunit;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 演示系统
    /// </summary>
    [CollectionDefinition("SystemModel")]
    public class BootstrapAdminDemoContext : ICollectionFixture<BASystemModelWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BASystemModelWebHost : BALoginWebHost
    {
        public BASystemModelWebHost() : base()
        {
            // 设置系统为演示模式
            using var db = Longbow.Data.DbManager.Create();
            db.Execute("Update Dicts Set Code = @2 where Category = @0 and Name = @1", "网站设置", "演示系统", "1");

            do
            {
                var task = Task.Delay(500);
                task.Wait();
                var dict = DataAccess.DictHelper.RetrieveSystemModel();
                if (dict) break;
            }
            while (true);
        }

        protected override void Dispose(bool disposing)
        {
            using var db = Longbow.Data.DbManager.Create();
            db.Execute("Update Dicts Set Code = @2 where Category = @0 and Name = @1", "网站设置", "演示系统", "0");

            do
            {
                var task = Task.Delay(500);
                task.Wait();
                var dict = DataAccess.DictHelper.RetrieveSystemModel();
                if (!dict) break;
            }
            while (true);
        }
    }
}
