using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Utils;

static class LocatorHelper
{
    public static IIPLocator CreateLocator(IServiceProvider provider)
    {
        var dictService = provider.GetRequiredService<IDict>();
        var providerName = dictService.GetIpLocatorName();
        var providerUrl = dictService.GetIpLocatorUrl(providerName ?? string.Empty);

        // TODO: 稍后完善 其余地理位置定位服务
        return providerName switch
        {
            "BaiDuIPSvr" => new BaiDuIPLocator(),
            "JuheIPSvr" => new BaiDuIPLocator(),
            "BaiDuIP138Svr" => new BaiDuIPLocator() { Url = providerUrl },
            _ => new DefaultIPLocator()
        };
    }
}
