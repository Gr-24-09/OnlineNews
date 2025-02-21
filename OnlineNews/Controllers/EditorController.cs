using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Services;

namespace OnlineNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IArticleService _articleService;
        private readonly ApplicationDbContext _context;
        private readonly ArticleService _articleService1;
        public EditorController(UserManager<User> userManager, IArticleService articleService, ApplicationDbContext context, ArticleService articleService1)
        {
            _userManager = userManager;
            _articleService = articleService;
            _context = context;
            _articleService1 = articleService1;
        }
        public IActionResult GetArticles()
        {
            var article = _context.Articles.OrderByDescending(x => x.Id).Take(6).ToList();
            return View(article);
        }

        [HttpGet("all")]
        public IActionResult GetAllArticles() 
        {
            var articles = _articleService1.GetAllArticles();
            return Ok(articles);
        }

        [HttpPost("approve/{id}")]
        public IActionResult ApproveArticle(int id)
        {
            var article = _articleService1.GetArticleById(id);
            if (article == null)
            {
                return NotFound("Article not found");
            }
            _articleService1.UppdateArticleApproval(id, false);
            return Ok("article rejected");
        }

    }
}
