using Bootstrap.Admin.Controllers;
using Bootstrap.DataAccess;
using Longbow.Caching;
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
                SetPrincipal(principal.Identity, actionContext);
            }
            else
            {
                if (actionContext.Request.Headers.Contains("Token"))
                {
                    try
                    {
                        var token = actionContext.Request.Headers.GetValues("Token").First();
                        if (!string.IsNullOrEmpty(token))
                        {
                            var auth = CacheManager.Get<LoginInfo>(token);
                            if (auth != null && !string.IsNullOrEmpty(auth.UserName))
                            {
                                SetPrincipal(new GenericIdentity(auth.UserName, "BAToken"), actionContext);
                                return true;
                            }
                        }
                    }
                    catch { }
                }
            }
            return base.IsAuthorized(actionContext);
        }

        private static void SetPrincipal(IIdentity identity, HttpActionContext actionContext)
        {
            var roles = RoleHelper.RetrieveRolesByUserName(identity.Name).Select(r => r.RoleName);
            actionContext.ControllerContext.RequestContext.Principal = new LgbPrincipal(identity, roles);
        }
    }
}