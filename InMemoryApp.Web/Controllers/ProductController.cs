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
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(5);

            //10 saniyelik bir süre verildi
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);

            //options.Priority = CacheItemPriority.Low;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}-->{value} => sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman",DateTime.Now.ToString(), options);

            return View();
        }
        public IActionResult Show()
        {
            //hafızadan okudum
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            return View();
        }
    }
}
