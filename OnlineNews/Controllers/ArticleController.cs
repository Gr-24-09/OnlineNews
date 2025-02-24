using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNews.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Models;

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

        [Authorize]
        public IActionResult Index(string name)
        {
            CategoryViewModel obj = new CategoryViewModel();
            obj.WorldArticles = _db.Articles.Where(a => a.Category.Name == "World").ToList();
            obj.EconomyArticles = _db.Articles.Where(a => a.Category.Name == "Economy").ToList();
            obj.TravelArticles = _db.Articles.Where(a => a.Category.Name == "Travel").ToList();
            obj.SwedenArticles = _db.Articles.Where(a => a.Category.Name == "Sweden").ToList();
            obj.ArtArticles = _db.Articles.Where(a => a.Category.Name == "Arts").ToList();
            obj.SportArticles = _db.Articles.Where(a => a.Category.Name == "Sport").ToList();
            obj.HealthArticles = _db.Articles.Where(a => a.Category.Name == "Health").ToList();
            return View(obj);
        }
        public IActionResult ArchivedArticles()
        {
            var articles1 = _db.Articles.Where(x => x.IsArchieved).Take(10).ToList();
            return View(articles1);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Editor,Writer")]
        public ViewResult AddArticle()
        {
            Article addArticle = new Article() { 
                ContentSummary = string.Empty,
                Content = string.Empty,
            };
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
        [Authorize(Roles = "Editor, Admin")]
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
       
        public IActionResult RemovedArticle()
        {
            return View();
        }


        [Authorize(Roles ="Admin, Editor")]
        public IActionResult Delete(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            _db.Articles.Remove(article);
            _db.SaveChanges();
            return RedirectToAction("RemovedArticle");
        }
        [Authorize(Roles = "Admin, Editor")]
        public IActionResult Edit(int id)
        {
            var data = _db.Articles.FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                return NotFound(); 
            }
            data.Categories = _db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(data);

        }

        [HttpPost]
        public IActionResult Edit(Article article)
        {
            if (!ModelState.IsValid)
            {
                article.Categories = _db.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View(article);
            }
            var data = _db.Articles.FirstOrDefault(x => x.Id == article.Id);
            if (data == null)
            {
                return NotFound();
            }
            data.Headline = article.Headline;
            data.ContentSummary = article.ContentSummary;
            data.Content = article.Content;
            data.ImageLink = article.ImageLink;
            data.LinkText = article.LinkText;
            data.EditorsChoice = article.EditorsChoice;
            data.IsArchieved = article.IsArchieved;
            if (!string.IsNullOrEmpty(article.ChosenCategory))
            {
                var categoryId = int.Parse(article.ChosenCategory);
                data.Category = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var articleDetails = _articleService.GetDetails(id);
            if (articleDetails == null)
            {
                return NotFound();
            }
            articleDetails.Views++;
            return View(articleDetails);
        }
        public IActionResult CategoryNews(int id)
        {
            var articles = _articleService.GetAllArticlesByItsCategory(id);
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            ViewData["CategoryName"] = category.Name;
            return View(articles);
        }
        public IActionResult SearchResult(string searchitem)
        {
            if (string.IsNullOrEmpty(searchitem))
            {
                return View(new List<Article>());
            }
            var articleList = _db.Articles.AsQueryable();
            articleList = articleList.Where(x =>
                x.Category.Name.Contains(searchitem) ||
                x.LinkText.Contains(searchitem) ||
                x.EditorsChoice.ToString().Contains(searchitem)||
                x.Likes.ToString().Contains(searchitem)||
                x.Views.ToString().Contains(searchitem)||
                x.Content.Contains(searchitem)||
                x.ContentSummary.Contains(searchitem)
            );
            var articles = articleList.ToList();
            return View(articles);
        }
        public IActionResult GetNumberOfLikesForAnArticle(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            var likesCount = article.Likes;
            return View(likesCount);
        }

        [Authorize]
        public IActionResult LikeAnArticle(int id)
        {
            var userId = _userManager.GetUserId(User);
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            article.Likes++;
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }
        [Authorize]
        public IActionResult DisLikeAnArticle(int id)
        {
            var userId = _userManager.GetUserId(User);
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            if (article.Likes > 0) // Ensure likes cannot go below zero
            {
                article.Likes--;  // Decrement the like count
            }
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Writer")]
        [HttpGet]
        public IActionResult EditAsWriter(int id)
        {
            var data = _db.Articles.FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                return NotFound();
            }
            data.Categories = _db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(data);
        }

        [HttpPost]
        public IActionResult EditAsWriter(Article article)
        {
            if (!ModelState.IsValid)
            {
                article.Categories = _db.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View(article);
            }
            var data = _db.Articles.FirstOrDefault(x => x.Id == article.Id);
            if (data == null)
            {
                return NotFound();
            }
            data.Headline = article.Headline;
            data.ContentSummary = article.ContentSummary;
            data.Content = article.Content;
            data.ImageLink = article.ImageLink;
            data.LinkText = article.LinkText;
            if (!string.IsNullOrEmpty(article.ChosenCategory))
            {
                var categoryId = int.Parse(article.ChosenCategory);
                data.Category = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        


    }
}
