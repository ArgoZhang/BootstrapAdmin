using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Bootstrap.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => builder.SetBasePath(AppContext.BaseDirectory).AddJsonFile("cache.json", true, true))
                .UseStartup<Startup>()
                .Build();
    }
}
