using Searching.Site.Models;
using System.Collections.Generic;

namespace Searching.Site.Services
{
    public interface ISearchService
    {
        IEnumerable<SearchResultModel> Search(string query);
    }
}