using Longbow.Security.Principal;
using Longbow.Web.Mvc;
using System;
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
                var roles = "Administrators;Users".Split(';'); //RoleHelper.RetrieveRolesByUserName();
                filterContext.HttpContext.User = new LgbPrincipal(filterContext.HttpContext.User.Identity, roles);
            }
            base.OnAuthorization(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected override bool AuthenticateRole(string userName)
        {
            Roles = "Administrators;SupperAdmin"; //RoleHelper.RetrieveRolesByUrl();
            return base.AuthenticateRole(userName);
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