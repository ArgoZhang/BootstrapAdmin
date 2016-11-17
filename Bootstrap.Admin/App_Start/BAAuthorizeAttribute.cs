using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    class BAAuthorizeAttribute : LgbAuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected override IEnumerable<string> RetrieveRolesByUserName(string userName)
        {
            return RoleHelper.RetrieveRolesByUserName(userName).Select(r => r.RoleName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override IEnumerable<string> RetrieveRolesByUrl(string url)
        {
            return RoleHelper.RetrieveRolesByUrl(url).Select(r => r.RoleName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
                return;
            }
            var view = new ViewResult();
            view.ViewName = "UnAuthorized";
            filterContext.Result = view;
        }
    }
}