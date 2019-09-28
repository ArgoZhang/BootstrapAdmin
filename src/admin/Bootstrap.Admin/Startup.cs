using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Bootstrap.Admin
{
    /// <summary>
    /// Startup 启动配置文件
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 获得 系统配置项 Iconfiguration 实例
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 服务容器注入方法
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddLogging(logging => logging.AddFileLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddCors();
            services.AddConfigurationManager();
            services.AddCacheManager();
            services.AddDbAdapter();
            services.AddIPLocator(DictHelper.ConfigIPLocator);
            services.AddOnlineUsers();
            services.AddSignalR().AddNewtonsoftJsonProtocol(op =>
            {
                op.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
                op.PayloadSerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddSignalRExceptionFilterHandler<SignalRHub>(async (client, ex) => await client.SendMessageBody(ex).ConfigureAwait(false));
            services.AddResponseCompression();
            services.AddBootstrapAdminAuthentication().AddGitee(OAuthHelper.Configure).AddGitHub(OAuthHelper.Configure);
            services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireBootstrapAdminAuthorizate().Build());
            services.AddSwagger();
            services.AddButtonAuthorization(MenuHelper.AuthorizateButtons);
            services.AddBootstrapAdminBackgroundTask();
            services.AddHttpClient<GiteeHttpClient>();
            services.AddAdminHealthChecks();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<SignalRExceptionFilter<SignalRHub>>();
            }).AddNewtonsoftJson(op =>
            {
                op.SerializerSettings.ContractResolver = new DefaultContractResolver();
                op.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                JsonConvert.DefaultSettings = () => op.SerializerSettings;
            });
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"), new QueryStringApiVersionReader("api-version"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 管道构建方法
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions() { ForwardedHeaders = ForwardedHeaders.All });
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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseBootstrapAdminAuthorization(RoleHelper.RetrievesByUserName, RoleHelper.RetrievesByUrl, AppHelper.RetrievesByUserName);
            app.UseSwagger(Configuration["SwaggerPathBase"].TrimEnd('/'));
            app.UseBootstrapHealthChecks();
            app.UseOnlineUsers(TraceHelper.Filter, TraceHelper.Save);
            app.UseCacheManager();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRHub>("/NotiHub");
                endpoints.MapHub<TaskLogHub>("/TaskLogHub");
                endpoints.MapHealthChecks("/healths");
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}
