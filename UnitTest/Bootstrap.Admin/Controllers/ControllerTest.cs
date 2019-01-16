using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Controllers
{
    public class ControllerTest : IClassFixture<BAWebHost>
    {
        protected HttpClient Client { get; }

        public ControllerTest(BAWebHost factory, string controller, bool login)
        {
            factory.ClientOptions.BaseAddress = new System.Uri($"http://localhost/{controller}/");
            Client = factory.CreateClient();

            if (login) factory.LoginAsync(Client).GetAwaiter().GetResult();
        }
    }
}
