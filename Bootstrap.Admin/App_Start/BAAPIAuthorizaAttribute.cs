using Longbow.Security.Principal;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;


namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class BAAPIAuthorizaAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;
            if (principal.Identity.IsAuthenticated)
            {
                if (LgbPrincipal.IsAdmin(principal.Identity.Name)) return true;
            }
            return base.IsAuthorized(actionContext);
        }
    }
}