using Bootstrap.DataAccess;
using Longbow.Security.Principal;
using Longbow.Web.Mvc;
using System;
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
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = filterContext.HttpContext.User.Identity.Name;
                var roles = RoleHelper.RetrieveRolesByUserName(username).Select(r => r.RoleName);
                filterContext.HttpContext.User = new LgbPrincipal(filterContext.HttpContext.User.Identity, roles);
            }
            base.OnAuthorization(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected override bool AuthenticateRole()
        {
            string url = string.Format("~/{0}/{1}", ControllerName, ActionName);
            Roles = string.Join(";", RoleHelper.RetrieveRolesByURL(url).Select(r => r.RoleName));
            return base.AuthenticateRole();
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