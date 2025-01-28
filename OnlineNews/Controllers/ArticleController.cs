using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNews.Service;
using Microsoft.AspNetCore.Identity;

namespace OnlineNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;


        public ArticleController(IArticleService articleService, ICategoryService categoryService, UserManager<User> userManager)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_articleService.GetAllArticles());
        }
        
        //[HttpGet]
        //public ViewResult AddArticle()
        //{
        //    Article addArticle = new Article();
        //    var categoryList = _articleService.GetAllCategory();
        //    foreach (var item in categoryList)
        //    {
        //        addArticle.Categories.Add(
        //            new SelectListItem { Value = item, Text = item }
        //            );
        //    }
        //    return View(addArticle);
        //}
        //[HttpPost]
        //public IActionResult AddArticle(Article newArticle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        newArticle.Category.Name = newArticle.ChosenCategory;
        //        var userId = _userManager.GetUserId(User)!;
        //        _articleService.AddArticle(newArticle, userId);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    var categoryList = _articleService.GetCategories();
        //    foreach (var item in categoryList)
        //    {
        //        newArticle.Categories.Add(
        //            new SelectListItem { Value = item, Text = item }
        //            );
        //    }
        //    return View(newArticle);
        //}
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
    }
}
