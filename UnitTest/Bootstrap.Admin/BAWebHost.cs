using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using UnitTest;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class BAWebHost : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// 
        /// </summary>
        public BAWebHost()
        {
            // Copy license
            TestHelper.CopyLicense();
        }

        public async Task<string> LoginAsync(HttpClient client)
        {
            var r = client.GetAsync("/Account/Login").GetAwaiter().GetResult();
            var view = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("Admin"), "userName");
            content.Add(new StringContent("123789"), "password");
            content.Add(new StringContent("true"), "remember");
            content.Add(new StringContent(antiToken), "__RequestVerificationToken");
            var resp = client.PostAsync("/Account/Login", content).GetAwaiter().GetResult();
            return await resp.Content.ReadAsStringAsync();
        }
    }
}
