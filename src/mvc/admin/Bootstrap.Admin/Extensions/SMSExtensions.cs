// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Web.SMS;
using Longbow.Web.SMS.Tencent;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 短信登录扩展类 
    /// </summary>
    public static class SMSExtensions
    {
        /// <summary>
        /// 注入短信登录服务到容器中
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSMSProvider(this IServiceCollection services)
        {
            services.AddTransient<ISMSProvider, TencentSMSProvider>();
            return services;
        }
    }
}
