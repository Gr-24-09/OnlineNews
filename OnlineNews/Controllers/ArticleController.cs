using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;

namespace OnlineNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
  

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View(_articleService.GetAllArticles());
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                _articleService.CreateArticle(article);
                return RedirectToAction("ArticleSuccess");
            }
            return View(article);
        }

        public IActionResult ArticleSuccess()
        {
            return View();
        }
        public IActionResult RemovedArticle()
        {
            return View();
        }

        
        public IActionResult DeleteArticle(int id)
        {
            _articleService.Delete(id);
            return RedirectToAction("GetAllArticles");
        }

        [HttpGet]
        public IActionResult EditArticle(int id)
        {
            var article = _articleService.GetDetails(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public IActionResult EditArticle(int id, Article article)
        {
            if (ModelState.IsValid)
            {
                var updateSuccessful = _articleService.Edit(id, article);

                if (!updateSuccessful)
                {
                    return NotFound();
                }

                return RedirectToAction("GetAllArticles");
            }

            return View(article);
        }

       
        public IActionResult Details(int id)
        {
            var articleDetails = _articleService.GetDetails(id);

            if (articleDetails == null)
            {
                return NotFound();
            }

            return View(articleDetails);
        }
    }
}
