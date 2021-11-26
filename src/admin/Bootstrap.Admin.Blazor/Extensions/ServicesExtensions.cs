using Bootstrap.DataAccess;
using BootstrapBlazor.Components;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// 添加示例后台任务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddBootstrapBlazorAdminServices(this IServiceCollection services)
        {
            // 增加后台任务服务
            services.AddTaskServices();

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
            //services.AddBootstrapAdminAuthentication(Configuration)
            //    .AddGitee(OAuthHelper.Configure)
            //    .AddGitHub(OAuthHelper.Configure)
            //    .AddTencent(OAuthHelper.Configure)
            //    .AddAlipay(OAuthHelper.Configure);
            //services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireBootstrapAdminAuthorizate().Build());
            //services.AddButtonAuthorization(MenuHelper.AuthorizateButtons);
            services.AddBootstrapAdminBackgroundTask();
            //services.AddHttpClient<GiteeHttpClient>();
            //services.AddAdminHealthChecks();
            //services.AddSMSProvider();

            //services.AddSwagger();
            //services.AddApiVersioning(option =>
            //{
            //    option.DefaultApiVersion = new ApiVersion(1, 0);
            //    option.ReportApiVersions = true;
            //    option.AssumeDefaultVersionWhenUnspecified = true;
            //    option.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"), new QueryStringApiVersionReader("api-version"));
            //});
            //services.AddExceptionless();
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add<BootstrapAdminAuthorizeFilter>();
            //    options.Filters.Add<ExceptionFilter>();
            //    options.Filters.Add<SignalRExceptionFilter<SignalRHub>>();
            //}).AddJsonOptions(op => op.JsonSerializerOptions.AddDefaultConverters());
            services.AddControllers().AddJsonOptions(op => op.JsonSerializerOptions.AddDefaultConverters());
            services.AddDisplayNames();
            // 增加 BootstrapBlazor 组件
            services.AddBootstrapBlazor();

            // 增加 PetaPoco ORM 数据服务操作类
            // 需要时打开下面代码
            //services.AddPetaPoco(option =>
            //{
            //    // 配置数据信息
            //    // 使用 SQLite 数据以及从配置文件中获取数据库连接字符串
            //    // 需要引用 Microsoft.Data.Sqlite 包，操作 SQLite 数据库
            //    // 需要引用 PetaPoco.Extensions 包，PetaPoco 包扩展批量插入与删除
            //    option.UsingProvider<SQLiteDatabaseProvider>()
            //          .UsingConnectionString(Configuration.GetConnectionString("bb"));
            //});

            return services;
        }
    }
}
