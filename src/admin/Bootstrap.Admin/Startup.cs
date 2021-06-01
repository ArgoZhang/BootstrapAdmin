using Bootstrap.DataAccess;
using Exceptionless;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;

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
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Enviroment = env;
        }

        /// <summary>
        /// 获得 当前运行时环境
        /// </summary>
        public IWebHostEnvironment Enviroment { get; }

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddLogging(logging => logging.AddFileLogger().AddCloudLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddCors();
            services.AddResponseCompression();

            services.AddCodePageProvider();
            services.AddCacheManager();
            services.AddDbAdapter();
            services.AddIPLocator(DictHelper.ConfigIPLocator);
            services.AddOnlineUsers();
            services.AddSignalR().AddJsonProtocol(op => op.PayloadSerializerOptions.AddDefaultConverters());
            services.AddSignalRExceptionFilterHandler<SignalRHub>(async (client, ex) => await client.SendMessageBody(ex).ConfigureAwait(false));
            services.AddBootstrapAdminAuthentication(Configuration)
                .AddGitee(OAuthHelper.Configure)
                .AddGitHub(OAuthHelper.Configure)
                .AddTencent(OAuthHelper.Configure)
                .AddAlipay(OAuthHelper.Configure);
            services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireBootstrapAdminAuthorizate().Build());
            services.AddButtonAuthorization(MenuHelper.AuthorizateButtons);
            services.AddBootstrapAdminBackgroundTask();
            services.AddHttpClient<GiteeHttpClient>();
            services.AddAdminHealthChecks();
            services.AddSMSProvider();

            services.AddSwagger();
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"), new QueryStringApiVersionReader("api-version"));
            });
            services.AddExceptionless();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<BootstrapAdminAuthorizeFilter>();
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<SignalRExceptionFilter<SignalRHub>>();
            }).AddJsonOptions(op => op.JsonSerializerOptions.AddDefaultConverters());
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options =>
            {
                if (Enviroment.IsDevelopment()) options.DetailedErrors = true;
            });
            services.AddDisplayNames();
            services.AddBootstrapBlazor();
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
            if (Configuration.GetValue("UseHttps", true)) app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseAutoGenerateDatabase();

            app.UseRouting();
            app.UseCors(builder => builder.WithOrigins(Configuration["AllowOrigins"].Split(',', StringSplitOptions.RemoveEmptyEntries)).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseExceptionless();
            app.UseBootstrapAdminAuthentication(RoleHelper.RetrievesByUserName, RoleHelper.RetrievesByUrl, AppHelper.RetrievesByUserName);
            app.UseAuthorization();
            app.UseSwagger(Configuration["SwaggerPathBase"].TrimEnd('/'));
            app.UseCacheManager();
            app.UseOnlineUsers(TraceHelper.Filter, TraceHelper.Save);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRHub>("/NotiHub");
                endpoints.MapHub<TaskLogHub>("/TaskLogHub");
                endpoints.MapBootstrapHealthChecks("/Healths", configure: op =>
                {
                    op.Predicate = new Func<HealthCheckRegistration, bool>(reg => DictHelper.RetrieveHealth());
                });
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
