using System.Linq;
using Umbraco.Web.WebApi;
using Umbraco.Web;
using Searching.Site.Services;
using System.Web.Mvc;

using System.Net.Http; // HttpResponseMessage için gerekli
using System.Net; // HttpStatusCode için gerekli

namespace Searching.Site.Controllers
{
    public class SearchApiController : UmbracoApiController
    {
        // ISearchService enjekte edin
        private readonly ISearchService _searchService;

        public SearchApiController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public HttpResponseMessage GetSearchResults(string query)
        {
            // Arama işlemi
            string[] docTypeAliases = new[] { "blogpost", "contentPage", "newsItem" };
            var searchResults = _searchService.GetContentSearchResults(query, docTypeAliases);


            // Sonuçları JSON formatında döndür
            var result = new
            {
                Results = searchResults.Select(x => new
                {
                    Title = x.Value<string>("title"),
                    Name = x.Name,
                    Url = x.Url()
                }),
                TotalItemCount = searchResults.Count()
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}