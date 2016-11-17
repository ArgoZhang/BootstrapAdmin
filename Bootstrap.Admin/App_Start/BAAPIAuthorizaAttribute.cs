using Bootstrap.DataAccess;
using Longbow.Security.Principal;
using System.Linq;
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
                var roles = RoleHelper.RetrieveRolesByUserName(principal.Identity.Name).Select(r => r.RoleName);
                actionContext.ControllerContext.RequestContext.Principal = new LgbPrincipal(principal.Identity, roles);
            }
            return base.IsAuthorized(actionContext);
        }
    }
}