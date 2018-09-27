using System.Threading.Tasks;

namespace SmokeBall.Search.Service.Interfaces
{
    public interface ISearchService
    {
        Task<string> FindRankingsAsync(string searchTerm, string url);
    }
}