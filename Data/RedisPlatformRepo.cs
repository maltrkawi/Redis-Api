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

            db.SetAdd("PlatformSet", platformStr);
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _conn.GetDatabase();
            var completeSet = db.SetMembers("PlatformSet");
            if(completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val)).ToList();

                return obj;
            }
            else
            {
                return null;
            }
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
