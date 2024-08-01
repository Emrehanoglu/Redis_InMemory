using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        //private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public ProductsController(IProductRepository productRepository, IDatabase database)
        {
            _productRepository = productRepository;

            //_redisService = redisService;
            //var db = _redisService.GetDb(0);
            //db.StringSet("deneme","devam");

            _database = database;
            _database.StringSet("deneme2", "devam2");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await _productRepository.CreateAsync(product));
        }
    }
}
