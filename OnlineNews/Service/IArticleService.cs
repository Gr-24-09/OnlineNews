
using OnlineNews.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineNews.Interfaces
{
    public interface IArticleService
    {
        void Add(Article article);
        void Delete(int id);
        bool Edit(int id, Article updateArticle);
        Article GetDetails(int id);
        //List<Article> GetAllArticles()
    }
}
