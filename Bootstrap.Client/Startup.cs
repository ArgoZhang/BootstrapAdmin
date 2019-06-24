using Bootstrap.Client.DataAccess;
using Bootstrap.Security.DataAccess;
using Longbow.Web;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Bootstrap.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors();
            services.AddLogging(builder => builder.AddFileLogger());
            services.AddConfigurationManager();
            services.AddCacheManager();
            services.AddDbAdapter();
            services.AddSignalR().AddJsonProtocalDefault();
            services.AddResponseCompression();
            services.AddBootstrapAdminAuthentication();
            services.AddMvc(options =>
            {
                options.Filters.Add<BootstrapAdminAuthorizeFilter>();
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                JsonConvert.DefaultSettings = () => options.SerializerSettings;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
            app.UseCors(builder => builder.WithOrigins(Configuration["AllowOrigins"].Split(',', StringSplitOptions.RemoveEmptyEntries)).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseBootstrapAdminAuthentication(RoleHelper.RetrieveRolesByUserName, RoleHelper.RetrieveRolesByUrl, DbHelper.RetrieveAppsByUserName);
            app.UseCacheManagerCorsHandler();
            app.UseSignalR(routes => { routes.MapHub<SignalRHub>("/NotiHub"); });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
