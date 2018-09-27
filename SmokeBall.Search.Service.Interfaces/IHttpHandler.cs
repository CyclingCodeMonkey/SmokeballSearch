using System.Net.Http;
using System.Threading.Tasks;

namespace SmokeBall.Search.Service.Interfaces
{
    /// <summary>
    /// Interface to abstract HTTPClient - the primary purpose is to allow
    /// ease of testing of HttpWebProxy class without actually connecting
    /// to the internet, i.e. mock desired behaviours
    /// Implementing only what is needed for the solution
    /// </summary>
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}