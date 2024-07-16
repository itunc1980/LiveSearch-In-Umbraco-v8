using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Searching.Site.Controllers
{

    public class NewsController : UmbracoApiController
    {
        [HttpGet]
        public IEnumerable<NewsItem> GetNewsList()
        {
            // Get the news content node
            var newsNode = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("news").FirstOrDefault();

            // Get the news items
            var newsItems = newsNode.Children<IPublishedContent>().Select(x => new NewsItem
            {
                Id = x.Id,
                PageTitle = x.Value<string>("title"),
                PageSummary = x.Value<string>("summary"),
                PageImage = x.Value<IPublishedContent>("image").Url(),
                Date = x.Value<DateTime>("date")
            });

            // Return the news items as JSON
            return newsItems;
        }

        [HttpGet]
        public NewsItem GetNewsDetail(int id)
        {
            // Get the news item by id
            var newsItem = Umbraco.Content(id);

            // Return the news item as JSON
            return new NewsItem
            {
                Id = newsItem.Id,
                PageTitle = newsItem.Value<string>("title"),
                PageSummary = newsItem.Value<string>("summary"),
                PageContent = newsItem.Value<string>("content"),
                PageImage = newsItem.Value<IPublishedContent>("image").Url(),
                Date = newsItem.Value<DateTime>("date")
            };
        }
    }

    public class NewsItem
    {
        public int Id { get; set; }
        public string PageTitle { get; set; }
        public string PageSummary { get; set; }
        public string PageContent { get; set; }
        public string PageImage { get; set; }
        public DateTime Date { get; set; }
    }

}