using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace Bootstrap.DataAccess
{

    [Collection("SQLServerContext")]
    public class AuthButtonTest
    {
        [Fact]
        public void User_Ok()
        {
            Assert.False(MenuHelper.AuthorizateButtons(new FooHttpContext(), "~/Admin/Profiles1", "saveDisplayName"));
            Assert.False(MenuHelper.AuthorizateButtons(new FooHttpContext(), "~/Admin/Index", "saveDisplayName"));
        }

        private class FooHttpContext : HttpContext
        {
            public override IFeatureCollection Features => throw new NotImplementedException();

            public override HttpRequest Request => throw new NotImplementedException();

            public override HttpResponse Response => throw new NotImplementedException();

            public override ConnectionInfo Connection => throw new NotImplementedException();

            public override WebSocketManager WebSockets => throw new NotImplementedException();

            public override AuthenticationManager Authentication => throw new NotImplementedException();

            public override ClaimsPrincipal User { get; set; } = new ClaimsPrincipal(new System.Security.Principal.GenericIdentity("User"));

            public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override void Abort() => throw new NotImplementedException();
        }
    }
}
