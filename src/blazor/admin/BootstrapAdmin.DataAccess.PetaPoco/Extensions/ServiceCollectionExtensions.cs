using BootstrapAdmin.DataAccess.PetaPoco;
using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using BootstrapBlazor.DataAcces.PetaPoco.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetaPoco;
using PetaPoco.Providers;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IServiceCollection AddPetaPocoDataAccessServices(this IServiceCollection services, Action<IServiceProvider, IDatabaseBuildConfiguration> builder)
    {
        services.TryAddSingleton<IDatabase>(provider =>
        {
            var option = DatabaseConfiguration.Build();
            builder(provider, option);
            option.UsingDefaultMapper<BootstrapAdminConventionMapper>();
            var db = new Database(option);

            var logger = provider.GetRequiredService<ILogger<Database>>();
            db.ExceptionThrown += (sender, e) =>
            {
                var message = e.Exception.Format(new NameValueCollection()
                {
                    [nameof(db.LastCommand)] = db.LastCommand,
                    [nameof(db.LastArgs)] = string.Join(",", db.LastArgs)
                });
                logger.LogError(new EventId(1001, "GlobalException"), e.Exception, message);
            };
            var env = provider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                db.CommandExecuted += (sender, args) =>
                {
                    var parameters = new StringBuilder();
                    foreach (DbParameter p in args.Command.Parameters)
                    {
                        parameters.AppendFormat("{0}: {1}  ", p.ParameterName, p.Value);
                    }
                    logger.LogInformation(args.Command.CommandText);
                    logger.LogInformation(parameters.ToString());
                };
            };
            return db;
        });

        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        // 增加缓存服务
        services.AddCacheManager();

        // 增加业务服务
        services.AddSingleton<IApp, AppService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IException, ExceptionService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IUser, UserService>();
        return services;
    }
}
