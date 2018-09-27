using System.Threading.Tasks;

namespace SmokeBall.Search.Service.Interfaces
{
    public interface IHttpWebProxy
    {
        Task<string> ExecuteGoogleSearchAsync(string searchTerm, int limit=100);
    }
}