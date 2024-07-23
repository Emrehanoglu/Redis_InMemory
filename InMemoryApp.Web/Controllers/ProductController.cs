using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //hafızaya kaydettim
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //1 dakikalık bir süre verildi
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //10 saniyelik bir süre verildi
            options.SlidingExpiration = TimeSpan.FromSeconds(10);

            _memoryCache.Set<string>("zaman",DateTime.Now.ToString(), options);

            return View();
        }
        public IActionResult Show()
        {
            //hafızadan okudum
            _memoryCache.TryGetValue("zaman", out string zamancache);

            ViewBag.zaman = zamancache;
            return View();
        }
    }
}
