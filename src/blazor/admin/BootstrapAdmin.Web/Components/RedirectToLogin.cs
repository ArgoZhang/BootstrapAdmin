namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        [NotNull]
        private NavigationManager? Navigation { get; set; }

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            Navigation.NavigateTo($"/Account/Login", true);
        }
#else
        protected override void OnInitialized()
        {
            Navigation.NavigateTo($"/Account/Login", true);
        }
#endif
    }
}
