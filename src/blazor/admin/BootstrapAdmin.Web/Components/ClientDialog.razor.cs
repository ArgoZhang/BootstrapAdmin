using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Validators;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ClientDialog
{
    /// <summary>
    ///
    /// </summary>
    [Parameter]
    [NotNull]
    public ClientApp? Value { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public EventCallback<ClientApp> ValueChanged { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public Func<ClientApp, Task>? OnSave { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    [NotNull]
    public Func<Task>? OnClose { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [NotNull]
    private List<IValidator>? Validators { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Validators = new List<IValidator>();
        if (Value.AppId == null)
        {
            Validators.Add(new AppIdValidator(DictService));
        }
    }

    private Task OnSaveCleint(EditContext context)
    {
        if (OnSave != null)
        {
            OnSave(Value);
        }
        return Task.CompletedTask;
    }

    private async Task OnClickClose()
    {
        if (OnClose != null)
        {
            await OnClose();
        }
    }
}
