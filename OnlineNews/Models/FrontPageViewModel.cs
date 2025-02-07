using OnlineNews.Models.Database;

namespace OnlineNews.Models
{
    public class FrontPageViewModel
    {

        public List<Article> Mostpopular { get; set; } = new List<Article>();
        public List<Article> LatestNews { get; set; } = new List<Article>();
        public List<Article> EditorsChoice{ get; set; } = new List<Article>();
       
    }
}
