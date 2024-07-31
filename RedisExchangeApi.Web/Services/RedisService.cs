using StackExchange.Redis;

namespace RedisExchangeApi.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        //db baglantısı yapıldı
        //uygulama ayaga kalktıgında direkt olarak db ile haberleşecek
        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString);
        }

        //16 tane db vardı, hangi db 'ye baglansın
        //belirtmediğim için ilk db 'ye yazmaya baslar
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
