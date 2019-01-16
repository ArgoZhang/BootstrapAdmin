using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class ApiTest : IClassFixture<BAWebHost>
    {
        protected HttpClient Client { get; }

        public ApiTest(BAWebHost factory, string view, bool login)
        {
            factory.ClientOptions.BaseAddress = new System.Uri($"http://localhost/api/{view}");
            Client = factory.CreateClient();

            if (login) factory.LoginAsync(Client).GetAwaiter();
        }
    }
}
