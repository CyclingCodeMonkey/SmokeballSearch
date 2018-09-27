using SmokeBall.Search.Models.Interfaces;

namespace SmokeBall.Search.Models
{
    public class SearchResult : ISearchResult
    {
        public int Position { get; set; }
        public string Url { get; set; }
    }
}
