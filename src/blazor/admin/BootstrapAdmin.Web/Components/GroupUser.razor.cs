namespace BootstrapAdmin.Web.Components
{
    public partial class GroupUser
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        [EditorRequired]
        public string? GroupId { get; set; }
    }
}
