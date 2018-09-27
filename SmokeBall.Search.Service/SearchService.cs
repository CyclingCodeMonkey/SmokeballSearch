using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmokeBall.Search.Models.Interfaces;
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

            var rankings = FindMatching(searchResults, url);
            return rankings.Any() 
                ? string.Join(", ", rankings) : "0";
        }

        private IList<int> FindMatching(IList<ISearchResult> resultList, string url)
        {
            var results = new ConcurrentBag<int>();
            Parallel.ForEach(resultList, (result) =>
            {
                if (result.Url.Contains(url, StringComparison.CurrentCultureIgnoreCase))
                {
                    results.Add(result.Position);
                }
            });
            return results.OrderBy(r=>r).ToList();
        }
    }
}