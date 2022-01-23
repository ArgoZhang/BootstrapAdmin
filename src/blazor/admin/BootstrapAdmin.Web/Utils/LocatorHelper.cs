using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Utils;

static class LocatorHelper
{
    public static IIPLocator CreateLocator(IServiceProvider provider)
    {
        var dictService = provider.GetRequiredService<IDict>();
        var providerName = dictService.GetIpLocatorName();
        var providerUrl = dictService.GetIpLocatorUrl(providerName ?? string.Empty);

        return providerName switch
        {
            "BaiDuIPSvr" => new BaiDuIPLocator(),
            "JuheIPSvr" => new BaiDuIPLocator(),
            "BaiDuIP138Svr" => new BaiDuIPLocator() { Url = providerUrl },
            _ => new DefaultIPLocator()
        };
    }
}
