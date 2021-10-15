using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, string authorizationToken = null,
            string content = "", string mediaType = MediaTypeNames.Application.Json);
    }
}