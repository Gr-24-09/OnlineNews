using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Service;


namespace OnlineNews.Services
{

    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;
        //private readonly ICategoryService _categoryService;

        public ArticleService(ApplicationDbContext db /*,/*ICategoryService categoryService*//*/*/)
        {
            _db = db;
            //_categoryService = categoryService;
        }

        public void AddArticle(Article newarticle,string authorId)
        {

            newarticle.PublishedDate = DateTime.Now;
            newarticle.Author = _db.Users.Find(authorId);
            newarticle.Category = (Category)_db.Categories.Where(c => c.Name == newarticle.Category.Name);
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
        public Article GetDetails(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);

            return article;
        }

    }
        
}
