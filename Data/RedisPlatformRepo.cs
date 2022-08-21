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
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            var db = _conn.GetDatabase();
            var platformStr = JsonSerializer.Serialize(platform);

            //db.StringSet(platform.Id, platformStr);
            //db.SetAdd("PlatformSet", platformStr);
            db.HashSet("hashplatform", new HashEntry[]
            {
                new HashEntry(platform.Id, platformStr)
            });
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _conn.GetDatabase();

            //var completeSet = db.SetMembers("PlatformSet");
            //if (completeSet.Length > 0)
            //{
            //    var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val)).ToList();

            //    return obj;
            //}
            //else
            //{
            //    return null;
            //}

            var completeHash = db.HashGetAll("hashplatform");
            if(completeHash != null)
            {
                var obj = Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
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

            //var platform = db.StringGet(id);
            var platform = db.HashGet("hashplatform", id);
            if (!string.IsNullOrEmpty(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform);
            }

            return null;
        }
    }
}
