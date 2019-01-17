using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        static BAWebHost()
        {
            // Copy license
            TestHelper.CopyLicense();
        }

        public BAWebHost()
        {
            Login();
        }

        public new HttpClient CreateClient() => CreateClient("http://localhost/api/");

        public HttpClient CreateClient(string baseAddress)
        {
            var client = CreateDefaultClient(new RedirectHandler(7), new CookieContainerHandler(cookie));
            client.BaseAddress = new Uri(baseAddress);
            return client;
        }

        private CookieContainer cookie = new CookieContainer();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var sqlConnectionStrings = "Data Source=.;Initial Catalog=UnitTest;User ID=sa;Password=sa";
            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("ConnectionStrings:ba", sqlConnectionStrings)
            }));
        }

        public string Login(string userName = "Admin", string password = "123789")
        {
            if (cookie.Count == 2) return "";

            var client = CreateClient("http://localhost/Account/Login");
            var r = client.GetAsync("");
            r.Wait();

            var tv = r.Result.Content.ReadAsStringAsync();
            tv.Wait();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var view = tv.Result;
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(userName), "userName");
            content.Add(new StringContent(password), "password");
            content.Add(new StringContent("true"), "remember");
            content.Add(new StringContent(antiToken), "__RequestVerificationToken");
            var resp = client.PostAsync("", content);
            resp.Wait();

            tv = resp.Result.Content.ReadAsStringAsync();
            tv.Wait();
            return tv.Result;
        }

        public void Logout()
        {
            cookie = new CookieContainer();
        }
    }
}
