using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class MembersController : Controller
{
    private readonly IDistributedCache _distributedCache;

    public MembersController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task<IActionResult> Index()
    {
        DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

        //datanın ömrü 1 dakika olacak
        cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

        Member member = new Member{ Id=1, Name="Emre",Surname="Hanoglu"};

        string jsonMember = JsonConvert.SerializeObject(member);

        await _distributedCache.SetStringAsync("member:1",jsonMember,cacheEntryOptions);

        return View();
    }
    public IActionResult Show()
    {
        string jsonMember = _distributedCache.GetString("member:1");

        Member member = JsonConvert.DeserializeObject<Member>(jsonMember);

        ViewBag.member = member;

        return View();
    }
    public IActionResult Index2()
    {
        DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

        //datanın ömrü 1 dakika olacak
        cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

        Member member = new Member { Id = 1, Name = "arda", Surname = "Hanoglu" };

        string jsonMember = JsonConvert.SerializeObject(member);

        Byte[] byteMember = Encoding.UTF8.GetBytes(jsonMember);

        _distributedCache.Set("member:1", byteMember, cacheEntryOptions);

        return View();
    }
    public IActionResult Show2()
    {
        Byte[] byteMember = _distributedCache.Get("member:1");

        string jsonMember = Encoding.UTF8.GetString(byteMember);

        Member member = JsonConvert.DeserializeObject<Member>(jsonMember);

        ViewBag.member = member;

        return View();
    }
}
