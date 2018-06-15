using Bootstrap.DataAccess;
using Bootstrap.Security.Filter;
using Bootstrap.Security.Middleware;
using Longbow.Cache;
using Longbow.Cache.Middleware;
using Longbow.Configuration;
using Longbow.Data;
using Longbow.Logging;
using Longbow.Web;
using Longbow.Web.WebSockets;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace Bootstrap.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddLogging(builder => builder.AddFileLogger().AddDBLogger(ExceptionHelper.Log));
            services.AddConfigurationManager();
            services.AddCacheManager();
            services.AddDBAccessFactory();
            services.AddDataProtection(op => op.ApplicationDiscriminator = "BootstrapAdmin")
                .SetApplicationName("__bd__")
                .DisableAutomaticKeyGeneration()
                .PersistKeysToFileSystem(new DirectoryInfo(ConfigurationManager.AppSettings["KeyPath"]));
            services.AddMvc(options =>
            {
                options.Filters.Add<BootstrapAdminAuthorizeFilter>();
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                JsonConvert.DefaultSettings = () => options.SerializerSettings;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
            app.UseCors(builder => builder.WithOrigins(Configuration["AllowOrigins"].Split(',', StringSplitOptions.RemoveEmptyEntries)).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseBootstrapRoleAuthorization();
            app.UseWebSocketHandler();
            app.UseCacheManagerCorsHandler();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
