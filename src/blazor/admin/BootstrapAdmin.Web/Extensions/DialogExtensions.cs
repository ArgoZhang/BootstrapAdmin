using BootstrapAdmin.Web.Components;

namespace BootstrapAdmin.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class DialogExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="title"></param>
    /// <param name="items"></param>
    /// <param name="value"></param>
    /// <param name="saveCallback"></param>
    /// <returns></returns>
    public static Task ShowAssignmentDialog(this DialogService dialogService, string title, List<SelectedItem> items, List<string> value, Func<Task<bool>> saveCallback, ToastService? toast) => dialogService.ShowSaveDialog<Assignment>(title,
        async () =>
        {
            var ret = await saveCallback();
            if (toast != null)
            {
                if (ret)
                {
                    await toast.Success("分配操作", "操作成功！");
                }
                else
                {
                    await toast.Error("分配操作", "操作失败，请联系相关管理员！");
                }
            }
            return ret;
        },
        new Dictionary<string, object?>
        {
            [nameof(Assignment.Items)] = items,
            [nameof(Assignment.Value)] = value,
            [nameof(Assignment.OnValueChanged)] = new Action<List<string>>(v =>
            {
                value.Clear();
                value.AddRange(v);
            })
        });
}
