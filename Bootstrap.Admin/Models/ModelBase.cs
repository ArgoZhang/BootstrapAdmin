using Bootstrap.DataAccess;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ModelBase()
        {
            Title = DictHelper.RetrieveWebTitle();
            Footer = DictHelper.RetrieveWebFooter();
            Theme = DictHelper.RetrieveActiveTheme();
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Footer { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Theme { get; private set; }
    }
}