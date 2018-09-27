using System.Collections.Generic;
using SmokeBall.Search.Models.Interfaces;

namespace SmokeBall.Search.Service.Interfaces
{
    public interface IHtmlParser
    {
        IList<ISearchResult> GetGoogleSearchResults(string inputHtml);
    }
}