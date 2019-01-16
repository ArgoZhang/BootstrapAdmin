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

        public async Task<string> LoginAsync(HttpClient client, string userName = "Admin", string password = "123789")
        {
            var r = await client.GetAsync("/Account/Logout");
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(userName), "userName");
            content.Add(new StringContent(password), "password");
            content.Add(new StringContent("true"), "remember");
            content.Add(new StringContent(antiToken), "__RequestVerificationToken");
            var resp = await client.PostAsync("/Account/Login", content);
            return await resp.Content.ReadAsStringAsync();
        }
    }
}
