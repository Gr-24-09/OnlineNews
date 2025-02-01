using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using System.Linq;


namespace OnlineNews.Services
{

    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;
        public ArticleService(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void AddArticle(Article newarticle,string authorId)
        {
            newarticle.PublishedDate = DateTime.Now;
            newarticle.Author = _db.Users.Find(authorId);
            newarticle.Category = _db.Categories.Where(c => c.Name == newarticle.Category.Name).First();
            _db.Articles.Add(newarticle);
            _db.SaveChanges();
        }
        public void EditArticle(Article article)
        {
            _db.Articles.Update(article);
            _db.SaveChanges();
        }
        public void Delete(int id)
        {
            var Article = _db.Articles.FirstOrDefault(a => a.Id == id);
            _db.Articles.Remove(Article);
            _db.SaveChanges();
        }
        public List<Article> GetAllArticles()
        {
            var articles = _db.Articles.ToList();
            return articles;
        }
        public List<string> GetAllCategories()
        {
            List<string> categoryList = new List<string>();
            categoryList = _db.Categories.Select(c=>c.Name).ToList();
            return categoryList;
        }
        public Article GetDetails(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            return article;
        }
        public List<Article> Mostpopular()
        {
            var articles = _db.Articles.OrderByDescending(x => x.Views).Take(5).ToList();
            return articles;
        }
        public List<Article> EditorsChoice()
        {
          var articles = _db.Articles.Where(x => x.EditorsChoice).ToList();
          return articles;
        }
       public  List<Article> LatestNews() 
       {
            var articles = _db.Articles.OrderByDescending(x => x.PublishedDate).Take(5).ToList();
            return articles;
       }
        public List<Article> GetAllArticlesByItsCategory(int categoryId) 
        {
            var articles = _db.Articles.Where(x => x.Category.Id == categoryId).ToList();
            return articles;
        }
        //public List<Article> SearchArticles(string searchitem)
        //{
        //    if (string.IsNullOrWhiteSpace(searchitem))
        //    {
        //        return new List<Article>();
        //    }

        //    // Perform the search on the client side (after pulling data into memory)
        //    var articles = _db.Articles
        //        .ToList() 
        //        .Where(a => a.Headline.Contains(searchitem, StringComparison.OrdinalIgnoreCase) ||
        //                    a.Content.Contains(searchitem, StringComparison.OrdinalIgnoreCase) ||
        //                    a.Category.Name.Contains(searchitem, StringComparison.OrdinalIgnoreCase))
        //        .ToList();

        //    return articles;
        //}



    }

}
