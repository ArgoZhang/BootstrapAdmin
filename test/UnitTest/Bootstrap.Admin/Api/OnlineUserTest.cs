using System;
using System.Reflection;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class OnlineUserTest
    {
        [Fact]
        public void PrivateUser_Dispose()
        {
            var assembly = Assembly.Load("Bootstrap.Admin");
            var t = Activator.CreateInstance(assembly.GetType("Bootstrap.Admin.Controllers.Api.OnlineUsersController", true, true));
            var userCacheType = t.GetType().GetNestedType("LoginUserCache", BindingFlags.NonPublic);
            var loginUserType = t.GetType().GetNestedType("LoginUser", BindingFlags.NonPublic);
            var loginUser = Activator.CreateInstance(loginUserType, true);
            var action = new Action(() => { });
            var userCache = Activator.CreateInstance(userCacheType, new object[] { loginUser, action });
            var mi = userCacheType.GetMethod("Dispose");
            mi.Invoke(userCache, null);
        }
    }
}
