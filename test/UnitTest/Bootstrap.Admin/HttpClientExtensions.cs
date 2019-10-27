using System.Net.Http;
using System.Threading.Tasks;

namespace Bootstrap.Admin
{
    /// <summary>
    /// HttpClient 扩展操作类
    /// </summary>
    public static class HttpClientExtensions
    {
        ///// <summary>
        ///// GetJson 异步方法
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="requestUri"></param>
        ///// <returns></returns>
        //public static async Task<T> GetAsJsonAsync<T>(this HttpClient client, string requestUri = null)
        //{
        //    var resp = await client.GetAsync(requestUri);
        //    var json = await resp.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions().AddDefaultConverters());
        //}

        ///// <summary>
        ///// PostJson 异步方法
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="requestUri"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static async Task<TRet> PostAsJsonAsync<TValue, TRet>(this HttpClient client, string requestUri, TValue t)
        //{
        //    var resp = await client.PostAsJsonAsync(requestUri, t);
        //    var json = await resp.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<TRet>(json, new JsonSerializerOptions().AddDefaultConverters());
        //}

        ///// <summary>
        ///// PostJson 异步方法
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static async Task<TRet> PostAsJsonAsync<TValue, TRet>(this HttpClient client, TValue t) => await PostAsJsonAsync<TValue, TRet>(client, string.Empty, t);

        ///// <summary>
        ///// DeleteJson 异步方法
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="requestUri"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static async Task<TRet> DeleteAsJsonAsync<TValue, TRet>(this HttpClient client, string requestUri, TValue t)
        //{
        //    var req = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        //    req.Content = new StringContent(JsonSerializer.Serialize(t));
        //    req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //    var resp = await client.SendAsync(req);
        //    var json = await resp.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<TRet>(json, new JsonSerializerOptions().AddDefaultConverters());
        //}

        ///// <summary>
        ///// DeleteJson 异步方法
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static async Task<TRet> DeleteAsJsonAsync<TValue, TRet>(this HttpClient client, TValue t) => await DeleteAsJsonAsync<TValue, TRet>(client, string.Empty, t);

        ///// <summary>
        ///// PutJson 异步方法
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="requestUri"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static async Task<TRet> PutAsJsonAsync<TValue, TRet>(this HttpClient client, string requestUri, TValue t)
        //{
        //    var resp = await client.PutAsJsonAsync(requestUri, t);
        //    var json = await resp.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<TRet>(json, new JsonSerializerOptions().AddDefaultConverters());
        //}

        ///// <summary>
        ///// PutJson 异步方法
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <typeparam name="TRet"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static async Task<TRet> PutAsJsonAsync<TValue, TRet>(this HttpClient client, TValue t) => await PutAsJsonAsync<TValue, TRet>(client, string.Empty, t);

        /// <summary>
        /// LoginAsync 异步方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task LoginAsync(this HttpClient client, string userName = "Admin", string password = "123789")
        {
            var r = await client.GetAsync("/Account/Login");
            var view = await r.Content.ReadAsStringAsync();
            var tokenTag = "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
            var index = view.IndexOf(tokenTag);
            view = view.Substring(index + tokenTag.Length);
            index = view.IndexOf("\" /></form>");
            var antiToken = view.Substring(0, index);

            var content = new MultipartFormDataContent
            {
                { new StringContent(userName), "userName" },
                { new StringContent(password), "password" },
                { new StringContent("true"), "remember" },
                { new StringContent(antiToken), "__RequestVerificationToken" }
            };
            await client.PostAsync("/Account/Login", content);
        }
    }
}
