using System.Net.Http;
using System.Threading.Tasks;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Service
{
    public class HttpClientHandler : IHttpHandler
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
    }
}