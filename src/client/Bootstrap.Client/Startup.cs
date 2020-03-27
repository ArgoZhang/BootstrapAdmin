using Bootstrap.Client.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Bootstrap.Client
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
            services.AddLogging(builder => builder.AddFileLogger());
            services.AddCors();
            services.AddResponseCompression();

            services.AddCodePageProvider();
            services.AddCacheManager();
            services.AddDbAdapter();
            services.AddBootstrapHttpClient();
            services.AddIPLocator(DictHelper.ConfigIPLocator);
            services.AddOnlineUsers();
            services.AddBootstrapAdminAuthentication(Configuration);
            services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireBootstrapAdminAuthorizate().Build());
            services.AddButtonAuthorization(MenuHelper.AuthorizateButtons);

            services.AddSendMail();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<BootstrapAdminAuthorizeFilter>();
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(op => op.JsonSerializerOptions.AddDefaultConverters());
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
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(builder => builder.WithOrigins(Configuration["AllowOrigins"].Split(',', StringSplitOptions.RemoveEmptyEntries)).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseBootstrapAdminAuthentication(RoleHelper.RetrievesByUserName, RoleHelper.RetrievesByUrl, AppHelper.RetrievesByUserName);
            app.UseAuthorization();
            app.UseCacheManager();
            app.UseOnlineUsers(callback: TraceHelper.Save);
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
