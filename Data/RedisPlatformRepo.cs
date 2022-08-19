using RedisApi.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisApi.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _conn;

        public RedisPlatformRepo(IConnectionMultiplexer conn)
        {
            _conn = conn;
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            var db = _conn.GetDatabase();
            var platformStr = JsonSerializer.Serialize(platform);
            db.StringSet(platform.Id, platformStr);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Platform? GetPlatformById(string id)
        {
            var db = _conn.GetDatabase();
            var platform = db.StringGet(id);
            if (!string.IsNullOrEmpty(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform);
            }

            return null;
        }
    }
}
