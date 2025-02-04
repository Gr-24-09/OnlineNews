
using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineNews.Interfaces
{
    public interface IArticleService
    {
        public void AddArticle(Article article, string authorId);
        public void Delete(int id);
        public Article GetDetails(int id);
        public List<Article> GetAllArticles();
        public List<string> GetAllCategories();
        public List<Article> Mostpopular();
        public List<Article> EditorsChoice();
        public List<Article> LatestNews();
        public List<Article> GetAllArticlesByItsCategory(int categoryId);
        
    }
}
