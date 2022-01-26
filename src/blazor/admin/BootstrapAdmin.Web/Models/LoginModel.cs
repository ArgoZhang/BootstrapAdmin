// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 登陆页面 Model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 验证码图床地址
        /// </summary>
        public string? ImageLibUrl { get; protected set; }

        /// <summary>
        /// 是否登录认证失败 为真时客户端弹出滑块验证码
        /// </summary>
        public bool AuthFailed { get; set; }
    }
}
