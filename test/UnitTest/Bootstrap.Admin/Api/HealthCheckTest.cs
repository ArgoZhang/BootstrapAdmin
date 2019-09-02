using Bootstrap.Admin.HealthChecks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using UnitTest;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class HealthCheckTest : IClassFixture<HealthCheckWebHost<HealthCheckStartup>>
    {
        private HttpClient client;

        public HealthCheckTest(HealthCheckWebHost<HealthCheckStartup> factory)
        {
            client = factory.CreateDefaultClient();
        }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await client.GetAsJsonAsync<object>("/Healths");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void UI_Ok()
        {
            var cates = await client.GetStringAsync("/Healths");
            Assert.Contains("TotalDuration", cates);

            // 测试数据库不能加载时健康检查
            var config = Longbow.Configuration.ConfigurationManager.AppSettings;
            config["DB:0:Enabled"] = "false";
            config["DB:4:Enabled"] = "true";
            config["DB:4:Widget"] = "Bootstrap.DataAccess.MongoDB1";
            cates = await client.GetStringAsync("/Healths");
            Assert.Contains("TotalDuration", cates);

            // 测试数据库连接字符串未配置
            config["DB:0:Enabled"] = "false";
            config["DB:2:Enabled"] = "true";
            config["DB:2:ConnectionStrings:ba"] = "Server=localhost;Database=UnitTest1;Uid=argozhang123;Pwd=argo@163.com;SslMode=none;";
            config["DB:4:Enabled"] = "false";
            config["DB:4:Widget"] = "Bootstrap.DataAccess.MongoDB";
            cates = await client.GetStringAsync("/Healths");
            Assert.Contains("TotalDuration", cates);

            config["DB:2:Enabled"] = "false";
            config["DB:2:ConnectionStrings:ba"] = "Server=localhost;Database=UnitTest;Uid=argozhang123;Pwd=argo@163.com;SslMode=none;";

            config["ConnectionStrings:ba"] = "";
            cates = await client.GetStringAsync("/Healths");
            Assert.Contains("TotalDuration", cates);
        }
    }

    public class HealthCheckStartup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public HealthCheckStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 获得 系统配置项 Iconfiguration 实例
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCacheManager();
            services.AddConfigurationManager();
            services.AddDbAdapter();
            var builder = services.AddHealthChecks();
            builder.AddCheck<DBHealthCheck>("db");
            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Principal.GenericIdentity("Argo"));
                await next();
            });
            app.UseHealthChecks("/Healths", new HealthCheckOptions()
            {
                ResponseWriter = (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new { report.Entries.Keys, Report = report }));
                },
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status200OK
                }
            });
            app.UseMvcWithDefaultRoute();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HealthCheckWebHost<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseContentRoot(TestHelper.RetrievePath($"UnitTest"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IWebHostBuilder CreateWebHostBuilder() => WebHost.CreateDefaultBuilder<TStartup>(null);
    }
}
