using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNews.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace OnlineNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;

        public ArticleController(IArticleService articleService, UserManager<User> userManager, ApplicationDbContext  db)
        {
            _articleService = articleService;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_articleService.GetAllArticles());
        }

        [Authorize]
        [HttpGet]
        public ViewResult AddArticle()
        {
            Article addArticle = new Article();
            var categoryList = _articleService.GetAllCategories();
            foreach (var item in categoryList)
            {
                addArticle.Categories.Add(
                    new SelectListItem { Value = item, Text = item }
                );
            }
            return View(addArticle);
        }
        [HttpPost]
        public IActionResult AddArticle(Article newArticle)
        {
            if (ModelState.IsValid)
            {
                newArticle.Category.Name = newArticle.ChosenCategory;
                var userId = _userManager.GetUserId(User)!;
                _articleService.AddArticle(newArticle, userId);
                return RedirectToAction("Index", "Home");
            }
            var categoryList = _articleService.GetAllCategories();
            foreach (var item in categoryList)
            {
                newArticle.Categories.Add(
                    new SelectListItem { Value = item, Text = item }
                );
            }
            return View(newArticle);
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

        public IActionResult Edit(int id)
        {
            //var data = _db.Articles.FirstOrDefault(x => x.Id == id);
            return View();

        }
        [HttpPost]
        public IActionResult Edit(Article article)
        {
            //var data = _db.Articles.FirstOrDefault(x => x.Id == article.Id);
            //if (data != null)
            //{
            //    data.Author = article.Author;
            //    data.PublishedDate = article.PublishedDate;
            //    data.ChosenCategory = article.ChosenCategory;
            //    data.Content = article.Content;
            //    data.ContentSummary = article.ContentSummary;
            //    data.ImageLink = article.ImageLink;
            //    data.Headline = article.Headline;
            //    _db.SaveChanges();
            //}

            _articleService.EditArticle(article);
            return RedirectToAction("Index", "Home");
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
        public IActionResult SearchResult(string byParameter)
        {
            if (string.IsNullOrEmpty(byParameter))
            {
                return View(new List<Article>());
            }
            var articleList = _db.Articles.AsQueryable();

            articleList = articleList.Where(x =>
                x.Id.ToString().Contains(byParameter)
            );
            var articles = articleList.ToList();
            return View(articles);
        }
    }
}
