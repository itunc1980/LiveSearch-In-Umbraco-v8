using Examine;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Examine;
using Umbraco.Web.WebApi;

namespace Searching.Site.Controllers
{
    public class SearchController : UmbracoApiController
    {
        private readonly IExamineManager _examineManager;

        public SearchController(IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        [HttpGet]
        public IEnumerable<SearchResult> GetSearchResults(string query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 3)
            {
                return Enumerable.Empty<SearchResult>();

            }

            if (_examineManager.TryGetIndex("ExternalIndex", out var index)) // İndeksi al
            {
                
                var searcher = index.GetSearcher(); // ISearcher'ı al

                if (searcher != null)
                {
                    var searchResults = searcher.CreateQuery("content")
                       .NodeTypeAlias("contentPage") // İlk NodeTypeAlias belirtilir
                        .Or() // Diğer NodeTypeAlias'ları eklemek için Or() kullanılır
                            .NodeTypeAlias("newsItem")
                            .Or()
                            .NodeTypeAlias("blogpost")
                            .Or()
                            .NodeTypeAlias("person")
                            .Or()
                            .NodeTypeAlias("product")
                        .And() // Tüm NodeTypeAlias'ları birleştirmek için And() kullanılır
                        .GroupedOr(new[] { "nodeName", "pageTitle", "bodyText" }, query)
                        .Execute(10);

                    return searchResults.Select(r => new SearchResult
                    {
                        Id = int.Parse(r.Id.ToString()),
                        Name = r.Values["nodeName"],
                        Url = Umbraco.Content(int.Parse(r.Id.ToString())).Url
                    });
                }
            }

            return Enumerable.Empty<SearchResult>();
        }

    }

    public class SearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}