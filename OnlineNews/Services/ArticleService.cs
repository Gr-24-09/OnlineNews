using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace OnlineNews.Services
{
    
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;
        private const string CookieConsentKey = "CookieConsent";
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
        public List<Article> GetAllArticles()
        {
            var articles = _db.Articles.ToList();
            return articles;
        }

        public Article GetArticleById(int id)
        {
            return _db.Articles.FirstOrDefault(a => a.Id == id);
        }

        public void UppdateArticleApproval(int id, bool isApproved)
        {
            var article = GetArticleById(id);
            if (article != null)
            {
                article.IsApproved = isApproved;
            }
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
            var articles3 = _db.Articles.OrderByDescending(x => x.Views).Take(10).ToList();
            return articles3;
        }
        public List<Article> EditorsChoice()
        {
            var articles1 = _db.Articles.Where(x => x.EditorsChoice).Take(10).ToList();
            return articles1;
        }
        public  List<Article> OneLatestNews() 
        {
            var articles2 = _db.Articles.OrderByDescending(x => x.PublishedDate).Take(1).ToList();
            return articles2;
        }
       public List<Article> SomeLatestNews()
       {
            var articles2 = _db.Articles.OrderByDescending(x => x.PublishedDate).Skip(1).Take(12).ToList();
            return articles2;
       }
        public List<Article> GetAllArticlesByItsCategory(int categoryId) 
        {
            var articles = _db.Articles.Where(x => x.Category.Id == categoryId).ToList();
            return articles;
        }
        public bool HasConsented(IHttpContextAccessor httpContextAccessor)
        {
            var consent = httpContextAccessor.HttpContext.Request.Cookies[CookieConsentKey];
            return consent == "true";
        }

        // Set cookie consent to true
        public void AcceptCookies(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(CookieConsentKey, "true", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                Secure = true // Ensures cookie is only sent over HTTPS
            });
        }
        public void DeclineCookies(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieConsentKey);
           
        }
    }

}
