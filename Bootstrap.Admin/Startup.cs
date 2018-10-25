using Bootstrap.DataAccess;
using Bootstrap.Security.Filter;
using Bootstrap.Security.Middleware;
using Longbow.Cache;
using Longbow.Cache.Middleware;
using Longbow.Configuration;
using Longbow.Data;
using Longbow.Logging;
using Longbow.Web;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bootstrap.Admin
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
            services.AddLogging(builder => builder.AddFileLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddConfigurationManager(Configuration);
            services.AddCacheManager(Configuration);
            services.AddDbAdapter(Configuration, () => { CacheManager.Clear(); CacheManager.CorsClear(new List<string>() { "*" }); });
            var dataProtectionBuilder = services.AddDataProtection(op => op.ApplicationDiscriminator = Configuration["ApplicationDiscriminator"])
                .SetApplicationName(Configuration["ApplicationName"])
                .PersistKeysToFileSystem(new DirectoryInfo(Configuration["KeyPath"]));
            if (Configuration["DisableAutomaticKeyGeneration"] == "True") dataProtectionBuilder.DisableAutomaticKeyGeneration();
            services.AddSignalR().AddJsonProtocalDefault();
            services.AddSignalRExceptionFilterHandler<SignalRHub>(async (client, ex) => await SignalRManager.Send(client, ex));
            services.AddMvc(options =>
            {
                options.Filters.Add<BootstrapAdminAuthorizeFilter>();
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<SignalRExceptionFilter<SignalRHub>>();
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                JsonConvert.DefaultSettings = () => options.SerializerSettings;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.Cookie.Path = "/");
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"), new QueryStringApiVersionReader("api-version"));
            });
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
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseBootstrapAdminAuthorization(userName => RoleHelper.RetrieveRolesByUserName(userName), url => RoleHelper.RetrieveRolesByUrl(url));
            app.UseCacheManagerCorsHandler();
            app.UseSignalR(routes => { routes.MapHub<SignalRHub>("/NotiHub"); });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BootstrapAdmin API V1");
            });
        }
    }
}