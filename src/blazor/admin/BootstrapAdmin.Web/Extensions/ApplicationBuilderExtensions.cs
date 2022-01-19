using BootstrapAdmin.Web;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplication UseBootstrapBlazorAdmin(this WebApplication builder)
    {
        // 开启健康检查
        builder.MapBootstrapHealthChecks();

        builder.UseBootstrapBlazor();

        builder.UseAuthentication();
        builder.UseAuthorization();

        builder.UseSwagger(builder.Configuration["SwaggerPathBase"].TrimEnd('/'));

        return builder;
    }
}
