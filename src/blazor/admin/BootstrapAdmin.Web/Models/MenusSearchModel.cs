using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

public class MenusSearchModel : ITableSearchModel
{
    [Display(Name = "名称")]
    public string? Name { get; set; }

    [Display(Name = "地址")]
    public string? Url { get; set; }

    /// <summary>
    /// 获得/设置 菜单分类, 0 表示系统菜单 1 表示用户自定义菜单
    /// </summary>
    [Display(Name = "类别")]
    public EnumNavigationCategory? Category { get; set; }

    /// <summary>
    /// 获得/设置 链接目标
    /// </summary>
    [Display(Name = "目标")]
    public string? Target { get; set; }

    /// <summary>
    /// 获得/设置 是否为资源文件, 0 表示菜单 1 表示资源 2 表示按钮
    /// </summary>
    [Display(Name = "类型")]
    public EnumResource? IsResource { get; set; }

    /// <summary>
    /// 获得/设置 所属应用程序，此属性由BA后台字典表分配
    /// </summary>
    [Display(Name = "所属应用")]
    public string? Application { get; set; }

    public IEnumerable<IFilterAction> GetSearchs()
    {
        var ret = new List<IFilterAction>();
        if (!string.IsNullOrEmpty(Name))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Name), Name, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Url))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Url), Url, FilterAction.Equal));
        }

        if (Category.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Category), Category.Value, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Application))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Application), Application, FilterAction.Equal));
        }

        if (IsResource.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.IsResource), IsResource.Value, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Target))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Target), Target, FilterAction.Equal));
        }
        return ret;
    }

    public void Reset()
    {
        Name = null;
        Url = null;
        Category = null;
        IsResource = null;
        Target = null;
        Application = null;
    }
}
