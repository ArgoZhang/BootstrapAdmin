using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class ApiWebHost : IClassFixture<BAWebHost>
    {
        protected HttpClient Client { get; }

        public ApiWebHost(BAWebHost factory, bool login)
        {
            factory.ClientOptions.BaseAddress = new System.Uri($"http://localhost/api/");
            Client = factory.CreateClient();

            if (login) factory.LoginAsync(Client).GetAwaiter().GetResult();
        }
    }
}
