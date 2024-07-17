using Searching.Site.Models.ViewModels;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace Searching.Site.Models
{
    public class SearchContentModel : ContentModel
    {
        public SearchContentModel(IPublishedContent content) : base(content)
        {
        }

        public SearchViewModel SearchViewModel { get; set; }

        public IEnumerable<SearchResultModel> SearchResults { get; set; }
    }
}