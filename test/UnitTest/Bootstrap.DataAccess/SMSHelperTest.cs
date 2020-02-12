using System.Text.Json;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class SMSHelperTest
    {
        [Fact]
        public void Send()
        {
            var payload = "{\"code\":1,\"msg\":\"验证码发送成功\",\"data\":\"ec22128a2968928d2e146bd9836cde7b\"}";
            var option = new JsonSerializerOptions();
            option.PropertyNameCaseInsensitive = true;
            var result = JsonSerializer.Deserialize<SMSResult>(payload, option);
            Assert.Equal(1, result.Code);

            var doc = JsonDocument.Parse(payload);
            result.Code = doc.RootElement.GetProperty("code").GetInt32();
            result.Data = doc.RootElement.GetProperty("data").GetString();
            Assert.Equal(1, result.Code);
        }

        private class SMSResult
        {
            public int Code { get; set; }

            public string Data { get; set; }
        }
    }
}
