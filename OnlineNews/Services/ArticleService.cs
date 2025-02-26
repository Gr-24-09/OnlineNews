using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CookieConsentKey = "CookieConsent";
        public ArticleService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
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
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                Console.WriteLine($"article not found with id {id}");
            }
            return article;

        }

        public void UpdateArticleApproval(int id, string status)
        {
            var article = GetArticleById(id);
            if (article != null)
            {
                article.ApprovalStatus = status;
                _db.SaveChanges();
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
            var articles3 = _db.Articles.OrderByDescending(x => x.Views).Take(12).ToList();
            return articles3;
        }
        public List<Article> EditorsChoice()
        {
            var articles1 = _db.Articles.Where(x => x.EditorsChoice).Take(10).ToList();
            return articles1;
        }
        public  List<Article> OneLatestNews() 
        {
            var articles2 = _db.Articles.OrderByDescending(x => x.PublishedDate).Take(2).ToList();
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
            var consent = _httpContextAccessor.HttpContext.Request.Cookies[CookieConsentKey];
            return consent == "true";
        }

        // Set cookie consent to true
        public void AcceptCookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("CookieConsent", "Accepted", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                Secure = true
            });
        }

        public void DeclineCookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("CookieConsent");
        }
        
        public void UpdateArticle(int id, Article updatedArticle)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.Likes = updatedArticle.Likes;
                article.Views = updatedArticle.Views;
                _db.SaveChanges();
            }
        }
        public void UpdateArticleViews(int id, int views)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                article.Views = views;
                _db.SaveChanges();
            }
        }

        public UserInteractionWithArticle GetUserArticleInteraction(string userId, int articleId)
        {
            return _db.UserInteractionWithArticles.FirstOrDefault(ai => ai.UserId == userId && ai.ArticleId == articleId);
        }

        public void AddArticleInteraction(string userId, int articleId, bool liked, bool disliked)
        {
            var interaction = new UserInteractionWithArticle
            {
                UserId = userId,
                ArticleId = articleId,
                Liked = liked,
                Disliked = disliked
            };
            _db.UserInteractionWithArticles.Add(interaction);
            _db.SaveChanges();
        }

        public void UpdateArticleInteraction(string userId, int articleId, bool liked, bool disliked)
        {
            var interaction = _db.UserInteractionWithArticles.FirstOrDefault(ai => ai.UserId == userId && ai.ArticleId == articleId);
            if (interaction != null)
            {
                interaction.Liked = liked;
                interaction.Disliked = disliked;
                _db.SaveChanges();
            }
        }

        public void UpdateArticleApproval(int id, bool isApproved)
        {
            throw new NotImplementedException();
        }
    }
}

