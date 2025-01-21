using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;

namespace OnlineNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ApplicationDbContext _db;

        public ArticleController(IArticleService articleService, ApplicationDbContext db)
        {
            _db = db;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
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
                _articleService.Add(article);
                return RedirectToAction("ArticleSuccess");
            }
            return View(article);
        }

        public IActionResult ArticleSuccess()
        {
            return View();
        }

      
        [HttpPost]
        public IActionResult DeleteArticle(int id)
        {
            _articleService.Delete(id);
            return RedirectToAction("GetAllArticles");
        }

        public IActionResult GetAllArticles()
        {
            return View(_articleService.GetAllArticles());
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

        [HttpGet]
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
