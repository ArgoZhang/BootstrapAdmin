using Bootstrap.DataAccess;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelBase
    {
        public ModelBase()
        {
            Title = DictHelper.RetrieveWebTitle();
            Footer = DictHelper.RetrieveWebFooter();
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        public string Footer { get; set; }
    }
}