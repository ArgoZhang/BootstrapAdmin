using Bootstrap.DataAccess;
using Longbow.Web;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

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
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors();
            services.AddLogging(builder => builder.AddFileLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddConfigurationManager();
            services.AddCacheManager();
            services.AddDbAdapter();
            services.AddIPLocator(DictHelper.ConfigIPLocator);
            services.AddOnlineUsers();
            services.AddSignalR().AddJsonProtocalDefault();
            services.AddSignalRExceptionFilterHandler<SignalRHub>((client, ex) => client.SendMessageBody(ex).ConfigureAwait(false));
            services.AddResponseCompression();
            services.AddBootstrapAdminAuthentication().AddGitee(OAuthHelper.Configure).AddGitHub(OAuthHelper.Configure);
            services.AddSwagger();
            services.AddButtonAuthorization(MenuHelper.AuthorizateButtons);
            services.AddBootstrapAdminBackgroundTask();
            services.AddHttpClient<GiteeHttpClient>();
            services.AddAdminHealthChecks();
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
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseAuthentication();
            app.UseBootstrapAdminAuthorization(RoleHelper.RetrievesByUserName, RoleHelper.RetrievesByUrl, AppHelper.RetrievesByUserName);
            app.UseBootstrapHealthChecks();
            app.UseOnlineUsers(TraceHelper.Filter, TraceHelper.Save);
            app.UseCacheManager();
            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRHub>("/NotiHub");
                routes.MapHub<TaskLogHub>("/TaskLogHub");
            });
            app.UseSwagger(Configuration["SwaggerPathBase"].TrimEnd('/'));
            app.UseMvcWithDefaultRoute();
        }
    }
}
