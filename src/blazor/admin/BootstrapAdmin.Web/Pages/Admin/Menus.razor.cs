using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Utils;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Menus
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [NotNull]
    private List<SelectedItem>? Targets { get; set; }

    [NotNull]
    private List<SelectedItem>? Apps { get; set; }

    [NotNull]
    private List<SelectedItem>? ParementMenus { get; set; }

    private ITableSearchModel? SearchModel { get; set; } = new MenusSearchModel();

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Targets = LookupHelper.GetTargets();
        Apps = DictService.GetApps().ToSelectedItemList();

        ParementMenus = NavigationService.GetAllMenus(AppContext.UserName).Where(s => s.ParentId == "0").Select(s => new SelectedItem(s.Id, s.Name)).ToList();
        ParementMenus.Insert(0, new SelectedItem("0", "请选择"));
    }

    private async Task OnAssignmentRoles(DataAccess.Models.Navigation menu)
    {
        var roles = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByMenuId(menu.Id);

        await DialogService.ShowAssignmentDialog($"分配角色 - 当前菜单: {menu.Name}", roles, values, () =>
        {
            var ret = RoleService.SaveRolesByMenuId(menu.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private Task<QueryData<Navigation>> OnQueryAsync(QueryPageOptions options)
    {
        var navs = NavigationService.GetAllMenus(AppContext.UserName);
        var menus = navs.Where(m => m.ParentId == "0");
        foreach (var item in menus)
        {
            item.HasChildren = navs.Any(i => i.ParentId == item.Id);
        }

        return Task.FromResult(new QueryData<Navigation>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = menus
        });
    }

    private Task<IEnumerable<Navigation>> OnTreeExpand(Navigation menu)
    {
        var navs = NavigationService.GetAllMenus(AppContext.UserName);
        return Task.FromResult(navs.Where(m => m.ParentId == menu.Id).OrderBy(m => m.Order).AsEnumerable());
    }
}
