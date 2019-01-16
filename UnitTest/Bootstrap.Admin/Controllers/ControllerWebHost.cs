using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class ControllerWebHost : IClassFixture<BAWebHost>
    {
        protected HttpClient Client { get; }

        public ControllerWebHost(BAWebHost factory, string controller, bool login)
        {
            factory.ClientOptions.BaseAddress = new System.Uri($"http://localhost/{controller}/");
            Client = factory.CreateClient();

            if (login) factory.LoginAsync(Client).GetAwaiter().GetResult();
        }
    }
}
