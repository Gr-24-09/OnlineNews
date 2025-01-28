using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;

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
