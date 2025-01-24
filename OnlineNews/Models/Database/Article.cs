using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineNews.Models.Database
{
    public class Article
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Publishing Date of Article is required.")]
        [Display(Name = "Publishing Date")]
        public DateTime PublishedDate { get; set; }
        public string LinkText { get; set; }
        
        [Required(ErrorMessage = "Headline of Article is required.")]
        [Display(Name = "Headline of Article")]
        [StringLength(100)]
        public string Headline { get; set; }

        [Required(ErrorMessage = "ContentSummary of Article is required.")]
        [Display(Name = "ContentSummary of Article")]
        public string ContentSummary { get; set; }

        [Required(ErrorMessage = "Content of Article is required.")]
        [Display(Name = "Content of Article")]
        public string Content { get; set; }

        public int Views { get; set; }
        public int Likes { get; set; }
        public string ImageLink { get; set; } = string.Empty;

        public Category Category { get; set; } = new Category();

        
        public string ChosenCategory { get; set; } = string.Empty;

        [NotMapped]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public User Author { get; set; } = new User();
        public bool IsArchieved { get; set; }

    }
}
