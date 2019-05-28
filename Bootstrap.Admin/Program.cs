using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseSentry("https://70bdfff562e84fa7b9a43d65924ab9ad@sentry.io/1469396").UseStartup<Startup>();
    }
}
