// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Services.SMS
{
    /// <summary>
    /// 短信登录接口
    /// </summary>
    public interface ISMSProvider
    {
        /// <summary>
        ///  手机下发验证码方法
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        Task<SMSResult> SendCodeAsync(string phoneNumber);

        /// <summary>
        ///  验证手机验证码是否正确方法
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        bool Validate(string phoneNumber, string code);

        /// <summary>
        /// 
        /// </summary>
        SMSOptions Options { get; }
    }
}
