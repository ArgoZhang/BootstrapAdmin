// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Services.SMS
{
    /// <summary>
    /// 短信网关配置类
    /// </summary>
    public class SMSOptions
    {
        /// <summary>
        /// 获得/设置 下发手机号码
        /// </summary>
        public string Phone { get; set; } = "";

        /// <summary>
        /// 获得/设置 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 获得/设置 验证码有效时长
        /// </summary>
        public TimeSpan Expires { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 获得/设置 角色集合
        /// </summary>
        public ICollection<string> Roles { get; } = new HashSet<string>();

        /// <summary>
        /// 获得/设置 登陆后首页
        /// </summary>
        public string HomePath { get; set; } = "";

        /// <summary>
        /// 获得/设置 默认授权 App
        /// </summary>
        public string App { get; set; } = "";

        /// <summary>
        /// 获得/设置 短信下发网关地址
        /// </summary>
        public string RequestUrl { get; set; } = "http://open.bluegoon.com/api/sms/sendcode";
    }
}
