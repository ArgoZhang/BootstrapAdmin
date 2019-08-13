namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// BootstrapAdmin 健康检查扩展类
    /// </summary>
    public static class HealthChecksAppBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseBootstrapHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("Healths");
            return app;
        }
    }
}
