// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.Web.Core.Services;
using BootstrapClient.Web.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using PetaPoco;
using PetaPoco.Providers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加示例后台任务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddBootstrapBlazorClient(this IServiceCollection services)
        {
            services.AddCors();
            services.AddResponseCompression();

            // 增加 BootstrapBlazor 组件
            services.AddBootstrapBlazor();

            // 增加认证授权服务
            services.AddBootstrapAdminSecurity<AdminService>();

            // 增加 BootstrapApp 上下文服务
            services.AddScoped<BootstrapAppContext>();

            // 增加 PetaPoco 数据服务
            services.AddPetaPocoDataAccessServices();

            return services;
        }
    }
}
