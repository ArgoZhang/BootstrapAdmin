using Bootstrap.Security;
using BootstrapBlazor.Components;
using System.Diagnostics.CodeAnalysis;

namespace Bootstrap.Admin.Blazor.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dict
    {

        private int[] PageSource { get; set; } = { 5, 20, 40 };

        [NotNull]
        private IDataService<BootstrapDict>? CustomerDataService { get; set; }
    }
}
