namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserRole
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
