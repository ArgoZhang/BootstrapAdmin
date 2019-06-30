using Bootstrap.Client.DataAccess;
using Bootstrap.Security.DataAccess;

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
            Title = DbHelper.RetrieveTitle();
            Footer = DbHelper.RetrieveFooter();
            Theme = DbHelper.RetrieveActiveTheme();
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
