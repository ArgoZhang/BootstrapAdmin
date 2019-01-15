using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    public class HomeTest : IClassFixture<BAWebHost>
    {
        private HttpClient _client;

        public HomeTest(BAWebHost factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(404)]
        [InlineData(500)]
        public async void Error_Ok(int errorCode)
        {
            var r = await _client.GetAsync($"/Home/Error/{errorCode}");
            Assert.True(r.IsSuccessStatusCode);
            var content = await r.Content.ReadAsStringAsync();
            if (errorCode == 0)
            {
                Assert.Contains("未处理服务器内部错误", content);
            }
            else if (errorCode == 404)
            {
                Assert.Contains("请求资源未找到", content);
            }
            else
            {
                Assert.Contains("服务器内部错误", content);
            }
        }
    }
}
