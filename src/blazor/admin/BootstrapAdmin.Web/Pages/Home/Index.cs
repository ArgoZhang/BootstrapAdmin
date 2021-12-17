namespace BootstrapAdmin.Web.Pages.Home
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/")]
    [Route("/Home")]
    [Route("/Home/Index")]
    public class Index : ComponentBase
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
            Navigation.NavigateTo($"/Admin/Index", true);
        }
#else
        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            Navigation.NavigateTo($"/Admin/Index", true);
        }
#endif
    }
}
