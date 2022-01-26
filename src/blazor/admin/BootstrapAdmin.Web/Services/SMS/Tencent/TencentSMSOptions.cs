// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Services.SMS.Tencent
{
    /// <summary>
    /// 腾讯短信配置实体类
    /// </summary>
    public class TencentSMSOptions : SMSOptions
    {
        /// <summary>
        /// 腾讯 AppId
        /// </summary>
        public string AppId { get; set; } = "";

        /// <summary>
        /// 腾讯 AppKey
        /// </summary>
        public string AppKey { get; set; } = "";

        /// <summary>
        /// 腾讯 模板 ID
        /// </summary>
        public int TplId { get; set; }

        /// <summary>
        /// 腾讯 应用签名
        /// </summary>
        public string Sign { get; set; } = "";

        /// <summary>
        /// 是否为 Debug 模式 默认为 False
        /// </summary>
        public bool Debug { get; set; }
    }
}
