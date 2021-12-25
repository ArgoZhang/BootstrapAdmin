using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;

namespace BootstrapAdmin.Web.Utils;

static class LookupHelper
{
    public static List<SelectedItem> GetTargets() => new List<SelectedItem>
    {
        new SelectedItem("_self", "本窗口"),
        new SelectedItem("_blank", "新窗口"),
        new SelectedItem("_parent", "父级窗口"),
        new SelectedItem("_top", "顶级窗口"),
    };

    public static List<SelectedItem> GetApps(IDict dictService) => dictService.GetApps().ToSelectedItemList();
}
