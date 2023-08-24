﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        [Required]
        public string Heading { get; set; }
        [Required]
        public string PageTitle { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string FeaturedImageUrl { get; set; }
        [Required]
        public string UrlHandle { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public bool Visible { get; set; }

        // Display Tags
        public IEnumerable<SelectListItem>? Tags { get; set; }
        // Collect Tags
        [Required]
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
