using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Searching.Site.Models.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Search Term")]
        [Required(ErrorMessage = "You must enter a search term")]
        public string Query { get; set; }

        public IEnumerable<SearchResultModel> SearchResults { get; set; }

        public int TotalResults { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // Toplam sayfa sayısını hesaplayan bir özellik
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalResults / PageSize);
            }
        }
    }
}