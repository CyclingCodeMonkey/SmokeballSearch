using System.Net.Http;
using System.Threading.Tasks;

namespace SmokeBall.Search.Service.Interfaces
{
    /// <summary>
    /// Interface to abstract HTTPClient
    /// Implementing only what is needed for the solution
    /// </summary>
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}