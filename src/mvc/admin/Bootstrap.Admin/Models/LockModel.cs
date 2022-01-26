// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 系统锁屏数据模型
    /// </summary>
    public class LockModel : HeaderBarModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userName"></param>
        public LockModel(string? userName) : base(userName)
        {

        }

        /// <summary>
        /// 获得/设置 返回路径
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// 获得/设置 认证方式 Cookie Mobile Gitee GitHub
        /// </summary>
        public string? AuthenticationType { get; set; }
    }
}
