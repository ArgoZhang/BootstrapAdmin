using Bootstrap.Security.Mvc;
using Longbow.Web.Mvc;
using System;
using System.Web.Mvc;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (filters == null) throw new ArgumentNullException("filters");
            filters.Add(new LgbHandleErrorAttribute());
            filters.Add(new BAAuthorizeAttribute());
        }
    }
}