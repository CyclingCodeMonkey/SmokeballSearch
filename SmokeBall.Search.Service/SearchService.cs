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
        private readonly IHtmlParser _htmlParser;
        private readonly IHttpWebProxy _httpWebProxy;

        public SearchService(IHttpWebProxy httpWebProxy, IHtmlParser htmlParser)
        {
            _httpWebProxy = httpWebProxy;
            _htmlParser = htmlParser;
        }

        /// <summary>
        ///     Search service to find the rankings of a particular url for a given search term.
        /// </summary>
        /// <param name="searchTerm">search term to lookup</param>
        /// <param name="url">url to find and match rankings</param>
        /// <param name="limitSearchResults">the number of search result items to be returned from the search engine (default:100)</param>
        /// <returns>
        ///     a string of comma separated numbers representing the position(s) of where the url was found, "0" if no ranking
        ///     found
        /// </returns>
        public async Task<string> FindRankingsAsync(string searchTerm, string url, int limitSearchResults = 100)
        {
            var result = await _httpWebProxy.ExecuteGoogleSearchAsync(searchTerm, limitSearchResults);
            var searchResults = _htmlParser.GetGoogleSearchResults(result);

            var rankings = FindMatching(searchResults, url);
            return rankings.Any()
                ? string.Join(", ", rankings)
                : "0";
        }

        /// <summary>
        ///     Finds all rankings that match the url passed in.
        ///     This method processes the entire list in parallel to efficently
        /// </summary>
        /// <param name="resultList">list of search results to process</param>
        /// <param name="url">the url to find</param>
        /// <returns>ordered list of rankings that matched the url</returns>
        private IList<int> FindMatching(IEnumerable<ISearchResult> resultList, string url)
        {
            var results = new ConcurrentBag<int>();
            Parallel.ForEach(resultList, result =>
            {
                if (result.Url.Contains(url, StringComparison.CurrentCultureIgnoreCase)) results.Add(result.Position);
            });
            return results.OrderBy(r => r).ToList();
        }
    }
}