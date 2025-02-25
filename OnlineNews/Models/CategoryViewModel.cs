using OnlineNews.Models.Database;

namespace OnlineNews.Models
{
    public class CategoryViewModel
    {
        public List<Article> SportArticles { get; set; } = new List<Article>();
        public List<Article> HealthArticles { get; set; } = new List<Article>();
        public List<Article> WorldArticles { get; set; } = new List<Article>();
        public List<Article> SwedenArticles { get; set; } = new List<Article>();
        public List<Article> TravelArticles { get; set; } = new List<Article>();
        public List<Article> ArtArticles { get; set; } = new List<Article>();
        public List<Article> EconomyArticles { get; set; } = new List<Article>();
        public List<Article> WeatherArticles { get; set; } = new List<Article>();


    }
}
