namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AdminCard
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public string? AuthorizeKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [EditorRequired]
        [NotNull]
        public string? HeaderText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
