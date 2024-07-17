using Searching.Site.Services;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Searching.Site.ApiControllers
{
    public class SearchApiController : UmbracoApiController
    {
        private readonly ISearchService _searchService;

        public SearchApiController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public IHttpActionResult GetSearchResults(string query)
        {
            var results = _searchService.Search(query);
            return Ok(results);
        }
    }
}