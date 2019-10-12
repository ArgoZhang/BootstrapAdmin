using Bootstrap.Admin.Controllers.Api;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class GiteeTest : ControllerTest
    {
        public GiteeTest(BAWebHost factory) : base(factory, "api/Gitee") { }

        [Fact]
        public async void Issues_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Issues");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Pulls_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Pulls");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Releases_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Releases");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Builds_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Builds");
            Assert.NotNull(cates);
        }

        [Fact]
        public void GetJsonAsync_Exception()
        {
            var t = typeof(GiteeController).GetMethod("GetJsonAsync", BindingFlags.NonPublic | BindingFlags.Static);
            t = t.MakeGenericMethod(new Type[] { typeof(string) });

            t.Invoke(null, new object[] {
                new Func<Task<string>>(() =>
                {
                    throw new TaskCanceledException();
                }),
                new Func<string, string>(content => {
                    return "";
                })
            });

            t.Invoke(null, new object[] {
                new Func<Task<string>>(()=> {
                    throw new Exception();
                }),
                new Func<string, string>(content => {
                    return "";
                })
            });
        }
    }
}
