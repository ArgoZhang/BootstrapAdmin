// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace Bootstrap.DataAccess.Helper
{
    /// <summary>
    /// 数据库自动生成帮助类
    /// </summary>
    public static class AutoDbHelper
    {
        /// <summary>
        /// 数据库检查方法
        /// </summary>
        public static void EnsureCreated(string folder) => DbContextManager.Create<AutoDB>()?.EnsureCreated(folder);
    }
}
