using Examine;
using Searching.Site.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Searching.Site.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public SearchService(IUmbracoContextFactory umbracoContextFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
        }

        public IEnumerable<SearchResultModel> Search(string query)
        {
            using (var context = _umbracoContextFactory.EnsureUmbracoContext())
            {
                if (!ExamineManager.Instance.TryGetIndex("ExternalIndex", out var index))
                {
                    return Enumerable.Empty<SearchResultModel>();
                }

                var searcher = index.GetSearcher();
                var searchCriteria = searcher.CreateQuery("content");
                var results = searchCriteria
                    .GroupedOr(new[] { "nodeName", "pageTitle", "bodyText" }, query.MultipleCharacterWildcard())
                    .Execute();

                List<SearchResultModel> searchResults = new List<SearchResultModel>();

                foreach (var result in results)
                {
                    var content = context.UmbracoContext.Content.GetById(int.Parse(result.Id));
                    if (content != null)
                    {
                        searchResults.Add(new SearchResultModel
                        {
                            Id = content.Id,
                            Name = content.Name,

                            Url = content.Url(mode: UrlMode.Relative),

                            Description = content.Value<string>("description")
                        });
                    }
                }

                return searchResults;
            }
        }
    }
}