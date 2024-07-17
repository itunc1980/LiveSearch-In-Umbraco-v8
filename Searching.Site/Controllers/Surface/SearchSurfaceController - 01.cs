using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Searching.Site.Models.ViewModels;
using Searching.Site.Services;
using Searching.Site.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Searching.Site.Controllers
{
    public class SearchSurfaceController : SurfaceController
    {
        private readonly ISearchService _searchService;

        public SearchSurfaceController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public ActionResult SubmitSearchForm(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var results = _searchService.Search(model.Query);
                model.SearchResults = results;

                // ViewData kullanarak SearchViewModel'i geçiyoruz
                ViewData["SearchViewModel"] = model;

                return PartialView("~/Views/Partials/SearchResults.cshtml", model);
            }

            return CurrentUmbracoPage();
        }
    }
}