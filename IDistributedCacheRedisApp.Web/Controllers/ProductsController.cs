using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IDistributedCache _distributedCache;

    public ProductsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public IActionResult Index()
    {
        DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

        //datanın ömrü 1 dakika olacak
        cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

        _distributedCache.SetString("isim","emre",cacheEntryOptions);

        return View();
    }

    public IActionResult Show()
    {
        string name = _distributedCache.GetString("isim");
        ViewBag.name = name;
        return View();
    }

    public IActionResult Remove()
    {
        _distributedCache.Remove("isim");

        return View();
    }
}
