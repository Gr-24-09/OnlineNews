using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNews.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Models;
using OnlineNews.Services;
using System.Drawing.Printing;

namespace OnlineNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IRequestService _requestService;
        private const int PageSize = 15; // Set the number of articles per page
        public ArticleController(IArticleService articleService, UserManager<User> userManager, ApplicationDbContext  db, IHttpContextAccessor httpContextAccessor, IRequestService requestService)
        {
            _articleService = articleService;
            _userManager = userManager;
            _db = db;
            _requestService= requestService;
        }

        [Authorize(Roles = "Editor,Admin,Writer")]
        public IActionResult Index(int page = 1, string categoryName = "")
        {
            var totalArticles = _db.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                totalArticles = totalArticles.Where(a => a.Category.Name == categoryName);
            }

            var totalPages = (int)Math.Ceiling(totalArticles.Count() / (double)PageSize);

            var articles = totalArticles
                .OrderByDescending(a => a.PublishedDate)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new CategoryViewModel
            {
                Articles = articles,
                CurrentPage = page,
                TotalPages = totalPages,
                CategoryFilter = categoryName
            };

            return View(viewModel);
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
        [Authorize(Roles = "Editor,Admin,Writer")]
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
        [Authorize(Roles ="Admin,Editor")]
        public IActionResult Delete(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            _db.Articles.Remove(article);
            _db.SaveChanges();
            return RedirectToAction("RemovedArticle");
        }
        [Authorize(Roles = "Admin,Editor")]
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
        //public IActionResult Details(int id)
        //{
        //    var articleDetails = _articleService.GetDetails(id);
        //    if (articleDetails == null)
        //    {
        //        return NotFound();
        //    }
        //    articleDetails.Views++;
        //    return View(articleDetails);
        //}
        public IActionResult Details(int id)
        {
            var articleDetails = _articleService.GetDetails(id);
            if (articleDetails == null)
            {
                return NotFound();
            }

            // Increment view count
            articleDetails.Views++;

            // Calculate the time difference
            var timeSpan = DateTime.Now - articleDetails.PublishedDate;
            string timeAgo;

            if (timeSpan.TotalDays >= 1)
            {
                timeAgo = $"{(int)timeSpan.TotalDays} day{(timeSpan.TotalDays > 1 ? "s" : "")} ago";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                timeAgo = $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours > 1 ? "s" : "")} ago";
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                timeAgo = $"{(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes > 1 ? "s" : "")} ago";
            }
            else
            {
                timeAgo = "just now";
            }

            // Pass the timeAgo string to the view (using ViewData or a model property)
            ViewData["TimeAgo"] = timeAgo;

            // Return the view with the article details
            return View(articleDetails);
        }

        public IActionResult CategoryNews(int id)
        {
            var articles = _articleService.GetAllArticlesByItsCategory(id);
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            ViewData["CategoryName"] = category.Name;
            return View(articles);
        }
        public async Task<IActionResult> SearchResult(string searchitem)
        {
            if (string.IsNullOrEmpty(searchitem))
            {
                return View(new List<Article>());
            }
            var articleList = _db.Articles.AsQueryable();
            articleList = articleList.Where(x =>
                x.Category.Name.Contains(searchitem) ||
                x.LinkText.Contains(searchitem) ||
                x.EditorsChoice.ToString().Contains(searchitem) ||
                x.Likes.ToString().Contains(searchitem) ||
                x.Views.ToString().Contains(searchitem)
            );
            var articles = articleList.ToList();
            if (!articles.Any())
            {
                ViewBag.msg = $"No results found for '{searchitem}'.";
            }
            else
            {
                ViewBag.msg = searchitem;
            }
            return View(articles);
        }
        public IActionResult GetNumberOfLikesForAnArticle(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            var likesCount = article.Likes;
            return View(likesCount);
        }
        [Authorize]
        [HttpPost]
        public IActionResult LikeAnArticle(int id)
        {
            var userId = _userManager.GetUserId(User);
            var articleDetails = _articleService.GetDetails(id);
            if (articleDetails == null)
            {
                return NotFound();
            }

            // Check if the user has already liked or disliked this article
            var userlike = _articleService.GetUserArticleInteraction(userId, id);

            if (userlike == null)
            {
                // If no interaction, create a new like entry
                _articleService.AddArticleInteraction(userId, id, true, false);  // Add a like, not a dislike
                articleDetails.Likes++;  // Increment the like count
            }
            else if (!userlike.Liked)
            {
                // If the user previously disliked, remove the dislike and add a like
                _articleService.UpdateArticleInteraction(userId, id, true, false);
                articleDetails.Likes++;  // Increment the like count (undo dislike)
            }
            // If the user already liked, do nothing.

            _articleService.UpdateArticle(id, articleDetails);  // Save changes to the article (like count)
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DisLikeAnArticle(int id)
        {
            var userId = _userManager.GetUserId(User);
            var articleDetails = _articleService.GetDetails(id);
            if (articleDetails == null)
            {
                return NotFound();
            }

            // Check if the user has already liked or disliked this article
            var userlike = _articleService.GetUserArticleInteraction(userId, id);

            if (userlike == null)
            {
                // If no interaction, create a new dislike entry
                _articleService.AddArticleInteraction(userId, id, false, true);  // Add a dislike, not a like
                articleDetails.Likes--;  // Decrease the like count
            }
            else if (!userlike.Disliked)
            {
                // If the user previously liked, remove the like and add a dislike
                _articleService.UpdateArticleInteraction(userId, id, false, true);
                articleDetails.Likes--;  // Decrease the like count (undo like)
            }
            // If the user already disliked, do nothing.

            _articleService.UpdateArticle(id, articleDetails);  // Save changes to the article (like count)
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
        public IActionResult AcceptCookies()
        {
            // Set a cookie indicating the user has accepted cookies
            Response.Cookies.Append("CookieConsent", "Accepted", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),  // Keep for a year
                HttpOnly = true, // To help with security
                Secure = true, // Only sent over HTTPS
            });

            TempData["Message"] = "You have accepted cookies.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeclineCookies()
        {
            // Remove the cookie or set it to a declined status
            Response.Cookies.Delete("CookieConsent");

            TempData["Message"] = "You have declined cookies.";
            return RedirectToAction("Index");
        }


    }
}
