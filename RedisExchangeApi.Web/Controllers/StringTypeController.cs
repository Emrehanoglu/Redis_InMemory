using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;

namespace RedisExchangeApi.Web.Controllers;

public class StringTypeController : Controller
{
    private readonly RedisService _redisService;

    public StringTypeController(RedisService redisService)
    {
        _redisService = redisService;
    }

    public IActionResult Index()
    {
        var db = _redisService.GetDb();
        db.StringSet("name","Emre Hanoglu");
        db.StringSet("name","Emre Hanoglu");
        return View();
    }
}
