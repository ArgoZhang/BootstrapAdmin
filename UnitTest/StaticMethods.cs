using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace UnitTest
{
    internal static class StaticMethods
    {
        public static void Setup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true, true)
              .Build();

            new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddConfigurationManager(config)
                .AddDbAdapter(config);
        }
    }
}
