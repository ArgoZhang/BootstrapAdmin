using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Bootstrap.Admin
{
    /// <summary>
    /// Swagger 扩展方法
    /// </summary>
    internal static class SwaggerExtensions
    {
        /// <summary>
        /// Swagger 中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathBase"></param>
        public static void UseSwagger(this IApplicationBuilder app, string pathBase)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "BootstrapAdmin API V1");
            });
        }

        /// <summary>
        /// 注入 Swagger 服务到容器内
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BootstrapAdmin API"
                });

                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Bootstrap.Admin.xml");
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });
        }
    }
}
