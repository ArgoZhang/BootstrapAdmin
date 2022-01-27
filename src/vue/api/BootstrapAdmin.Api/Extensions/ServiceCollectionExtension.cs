// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using PetaPoco;
using PetaPoco.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BootstrapAdmin.Api.Authencation;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBootstrapAdminService(this IServiceCollection services)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                var tokenValidateOption = new TokenValidateOption();
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = tokenValidateOption.Audience,//Audience
                    ValidIssuer = tokenValidateOption.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidateOption.SecurityKey)),//拿到SecurityKey
                };
                option.RequireHttpsMetadata = false;
            });

        services.AddPetaPocoDataAccessServices((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connString = configuration.GetConnectionString("bb");
            builder.UsingProvider<SQLiteDatabaseProvider>()
                   .UsingConnectionString(connString);
        });
        return services;
    }
}
