// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 重置用户操作类
    /// </summary>
    public static class ResetUserHelper
    {
        /// <summary>
        /// 保存需要重置用户
        /// </summary>
        /// <returns></returns>
        public static bool Save(ResetUser user)
        {
            user.ResetTime = DateTime.Now;
            return DbContextManager.Create<ResetUser>()?.Save(user) ?? false;
        }
    }
}
