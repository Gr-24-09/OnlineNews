
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
        public void EditArticle(Article article);
        public List<Article> Mostpopular();
        public List<Article> EditorsChoiced();
        public List<Article> LatestNews();
        public List<Article> World();
        public List<Article> Sweden();
        public List<Article> Travel();
        public List<Article> Culture();
        public List<Article> Business();
        public List<Article> Sport();

    }
}
