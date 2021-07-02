using Bootstrap.Admin.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class HealthCheckTest : IClassFixture<HealthCheckWebHost<HealthCheckStartup>>
    {
        private readonly HttpClient client;

        public HealthCheckTest(HealthCheckWebHost<HealthCheckStartup> factory)
        {
            client = factory.CreateDefaultClient();
        }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await client.GetFromJsonAsync<object>("/Healths");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void UI_Ok()
        {
            var cates = await client.GetStringAsync("/Healths");
            Assert.Contains("TotalDuration", cates);

            // 测试数据库不能加载时健康检查
            var config = HealthCheckStartup.Configuration;
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
        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCacheManager();
            services.AddDbAdapter();
            services.AddHealthChecks().AddCheck<DBHealthCheck>("db");
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                context.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Principal.GenericIdentity("Argo"));
                await next();
            });
            app.UseEndpoints(builder => builder.MapHealthChecks("/Healths", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { report.Status, report.TotalDuration }));
                },
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status200OK
                }
            }));
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
        protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder => builder.UseStartup<TStartup>());
    }
}
