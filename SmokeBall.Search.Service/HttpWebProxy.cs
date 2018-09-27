using System;
using System.IO;
using System.Threading.Tasks;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Service
{
    public class HttpWebProxy : IHttpWebProxy
    {
        private const string GoogleUrl = "https://www.google.com.au/search";
        private readonly IHttpHandler _httpHandler;

        public HttpWebProxy(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        /// <summary>
        ///     Execute a Google Search for the search term specified
        /// </summary>
        /// <param name="searchTerm">The search Term</param>
        /// <param name="limit">limit the number of results to be retuned</param>
        /// <returns></returns>
        public async Task<string> ExecuteGoogleSearchAsync(string searchTerm, int limit = 100)
        {
            // check and constrain the search results
            if (limit <= 0 || limit >= 1000)
                limit = 100;

            if (string.IsNullOrWhiteSpace(searchTerm))
                return string.Empty;

            var url = $"{GoogleUrl}?num={limit}&q={searchTerm}";
            return await SearchAsync(url);
        }
        
        private async Task<string> SearchAsync(string url)
        {
            var result = await _httpHandler.GetAsync(url);
            if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);

            var stream = await result.Content.ReadAsStreamAsync();
            using (var readStream = new StreamReader(stream))
            {
                return readStream.ReadToEnd();
            }
        }
    }
}