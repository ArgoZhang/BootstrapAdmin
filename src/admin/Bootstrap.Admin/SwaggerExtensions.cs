using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    internal static class SwaggerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathBase"></param>
        public static void UseSwagger(this IApplicationBuilder app, string pathBase)
        {
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), builder =>
            {
                builder.Use(async (context, next) =>
                {
                    if (!context.User.Identity.IsAuthenticated) await context.ChallengeAsync();
                    else await next();
                });
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "BootstrapAdmin API V1");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "BootstrapAdmin API"
                });

                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Bootstrap.Admin.xml");
                options.IncludeXmlComments(xmlPath);
                options.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
            });
        }
    }
}
