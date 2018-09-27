using System.Threading.Tasks;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Service
{
    public class SearchService : ISearchService
    {
        private readonly IHttpWebProxy _httpWebProxy;
        private readonly IHtmlParser _htmlParser;

        public SearchService(IHttpWebProxy httpWebProxy, IHtmlParser htmlParser)
        {
            _httpWebProxy = httpWebProxy;
            _htmlParser = htmlParser;
        }

        public async Task<string> FindRankingsAsync(string searchTerm, string url)
        {
            var result = await _httpWebProxy.ExecuteGoogleSearchAsync(searchTerm, 100);

            var searchResults = _htmlParser.GetGoogleSearchResults(result);

            return result;
        }
    }
}