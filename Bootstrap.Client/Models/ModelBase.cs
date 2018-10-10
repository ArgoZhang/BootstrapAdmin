using Bootstrap.Client.DataAccess;

namespace Bootstrap.Client.Models
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
            Title = DictHelper.RetrieveTitle();
            Footer = DictHelper.RetrieveFooter();
            Theme = DictHelper.RetrieveActiveTheme();
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Footer { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Theme { get; protected set; }
    }
}