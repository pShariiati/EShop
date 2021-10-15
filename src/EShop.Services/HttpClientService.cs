using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EShop.Services.Contracts;

namespace EShop.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method,
            string authorizationToken = null, string content = "", string mediaType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authorizationToken);
            }
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(url),
                Content = new StringContent(content, Encoding.UTF8, mediaType),
            };
            return await _httpClient.SendAsync(request);
        }
    }
}
