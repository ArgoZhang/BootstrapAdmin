using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bootstrap.Admin
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsJsonAsync<T>(this HttpClient client, string requestUri = null)
        {
            var resp = await client.GetAsync(requestUri);
            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<TRet> PostAsJsonAsync<TValue, TRet>(this HttpClient client, string requestUri, TValue t)
        {
            var resp = await client.PostAsJsonAsync(requestUri, t);
            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRet>(json);
        }

        public static async Task<TRet> DeleteAsJsonAsync<TValue, TRet>(this HttpClient client, string requestUri, TValue t)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            req.Content = new StringContent(JsonConvert.SerializeObject(t));
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var resp = await client.SendAsync(req);
            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRet>(json);
        }

        public static async Task<TRet> PutAsJsonAsync<TValue, TRet>(this HttpClient client, string requestUri, TValue t)
        {
            var resp = await client.PutAsJsonAsync(requestUri, t);
            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRet>(json);
        }
    }
}
