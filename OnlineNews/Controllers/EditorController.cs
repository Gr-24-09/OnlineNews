using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Services;

namespace OnlineNews.Controllers
{
    [ApiController]
    [Route("Editor")]
    public class EditorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IArticleService _articleService;
        private readonly ApplicationDbContext _db;
        

        public EditorController(UserManager<User> userManager, IArticleService articleService, ApplicationDbContext db)
        {
            _userManager = userManager;
            _articleService = articleService;
            _db = db;
            
        }


        [HttpGet]
        public IActionResult GetAllArticles(bool showRejected = false)
        {
            var articles = _articleService.GetAllArticles();
            if (showRejected) 
            {
                articles = articles.Where(a => !a.IsApproved).ToList();
            }
            else
            {
                articles = articles.Where(a => a.IsApproved).ToList();
            }

            return View(articles);
        }

        [HttpPost("approve/{id}")]
        public IActionResult ApproveArticle(int id)
        {
            var article = _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound("Article not found");
            }
            _articleService.UpdateArticleApproval(id, true);
            return RedirectToAction("GetAllArticles");
        }

        [HttpPost("reject/{id}")]
        public IActionResult RejectArticle(int id)
        {
            var article = _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound("Article not found");
            }
            _articleService.UpdateArticleApproval(id, false);
            return RedirectToAction("GetAllArticles", new { showRejected = true });
        }

        public IActionResult RejectedArticles(int id) 
        {
            var rejectedArticles = _articleService.GetAllArticles().Where(a => !a.IsApproved).ToList();
            return View(rejectedArticles);
        }


        //public enum ArticleStatus
        //{
        //    Waiting = 1,
        //    Pending = 2,
        //    Rejected = 3,
        //    Approved = 4
        //}
        
    }
}
