using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;


namespace OnlineNews.Services
{

    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;

        public ArticleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void CreateArticle(Article article)
        {
            _db.Articles.Add(article);
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
            //var qSyntax = from m in _db.Articles select m;
            //var AllArticlesQuey = _db.Articles.FromSqlRaw("select  * from Articles").ToList();
            return articles;
        }
        public Article GetDetails(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);

            return article;
        }

        public bool Edit(int id, Article updateArticle)
        {
            var article = _db.Articles.FirstOrDefault(c => c.Id == id);

            if (article == null)
            {
                return false;
            }
            article.Headline = updateArticle.Headline;
            article.Author = updateArticle.Author;
            article.Category = updateArticle.Category;
            article.PublishedDate = updateArticle.PublishedDate;
            article.ImageLink = updateArticle.ImageLink;
            article.Content = updateArticle.Content;
            article.ContentSummary = updateArticle.ContentSummary;
            _db.SaveChanges();
            return true;
        }
    }
}
