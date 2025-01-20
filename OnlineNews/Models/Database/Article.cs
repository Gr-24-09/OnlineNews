using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title of Article is required.")]
        [Display(Name = "Title of Article")]
        [StringLength(100)]
        public string Headline { get; set; }

        [Required(ErrorMessage = "Content of Article is required.")]
        [Display(Name = "Content of Article")]
        public string Content { get; set; }

        [Required(ErrorMessage = "ContentSummary of Article is required.")]
        [Display(Name = "ContentSummary of Article")]
        public string ContentSummary { get; set; }

        [Required(ErrorMessage = "Category of Article is required.")]
        [Display(Name = "Category")]
        [StringLength(50)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Publishing Date of Article is required.")]
        [Display(Name = "Publishing Date")]
        public DateTime DateStamp { get; set; }

        [Required(ErrorMessage = "Name of the Writer is required.")]
        [Display(Name = "Writer Name")]
        [StringLength(50)]
        public string Writer { get; set; }

        [Required(ErrorMessage = "Name of Location is required.")]
        [Display(Name = "Location Name")]
        [StringLength(50)]
        public string Location { get; set; }
        public string ImageLink { get; set; } = string.Empty;

    }
}
