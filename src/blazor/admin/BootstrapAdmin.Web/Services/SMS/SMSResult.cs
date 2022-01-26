// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Services.SMS
{
    /// <summary>
    /// 短信结果实体类
    /// </summary>
    public class SMSResult
    {
        /// <summary>
        /// 短信验证码是否发送成功
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 短信验证码返回数据
        /// </summary>
        public string? Data { get; set; }

        /// <summary>
        /// 短信失败原因提示信息
        /// </summary>
        public string? Msg { get; set; }
    }
}
