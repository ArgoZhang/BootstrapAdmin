namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserGroup
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        [EditorRequired]
        public string? UserName { get; set; }
    }
}
