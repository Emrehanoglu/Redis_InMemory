using RedisExampleApp.Api.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.Api.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository,
            RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(4);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            //once db 'ye eklendi
            var newProduct = await _productRepository.CreateAsync(product);

            //sonra cache 'e eklendi
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, 
                JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            //once git bak cache içerisinde ilgili key ile ilgili data var mı
            //yok ise önce git cachle
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            //data cache içerisinde var ise liste halinde return edelim
            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);

                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            //önce cache 'e bakalım
            //data var ise dönelim
            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            //data yok ise db 'ye bakalım
            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        //db 'den cache 'e yükle ve o datayı dön
        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return products;
        }
    }
}
