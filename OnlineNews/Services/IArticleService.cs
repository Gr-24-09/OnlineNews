
using OnlineNews.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineNews.Interfaces
{
    public interface IArticleService
    {
        void AddArticle(Article article, string authorId);
        void Delete(int id);
        Article GetDetails(int id);
        List<Article> GetAllArticles();
        void EditArticle(Article article);
    }
}
