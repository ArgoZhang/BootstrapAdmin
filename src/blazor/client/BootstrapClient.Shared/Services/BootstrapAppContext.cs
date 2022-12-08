// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.Extensions.Configuration;

namespace BootstrapClient.Web.Shared.Services
{
    /// <summary>
    /// AppContext 实体类
    /// </summary>
    public class BootstrapAppContext
    {
        /// <summary>
        /// 获得/设置 当前网站 AppId
        /// </summary>
        [NotNull]
        public string? AppId { get; }

        /// <summary>
        /// 获得/设置 当前登录账号
        /// </summary>
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 获得/设置 当前用户显示名称
        /// </summary>
        [NotNull]
        public string? DisplayName { get; internal set; }

        /// <summary>
        /// 获得/设置 应用程序基础地址 如 http://localhost:49185
        /// </summary>
        [NotNull]
        public Uri? BaseUri { get; set; }

        /// <summary>
        /// 获得/设置 后台程序基础地址 如 http://localhost:5210
        /// </summary>
        public string? AdminUrl { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public BootstrapAppContext(IConfiguration configuration)
        {
            AppId = configuration.GetValue<string?>("AppId", "BA");
        }
    }
}
