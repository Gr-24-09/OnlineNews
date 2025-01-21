using Microsoft.AspNetCore.Mvc;
using OnlineNews.Data;
using OnlineNews.Services;

namespace OnlineNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ArticleService _articleService;
        
        public ArticleController(ApplicationDbContext context, ArticleService articleService)
        {
            _context = context;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
