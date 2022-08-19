using Microsoft.AspNetCore.Mvc;
using RedisApi.Data;
using RedisApi.Models;

namespace RedisApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;

        public PlatformsController(IPlatformRepo platformRepo)
        {
            _platformRepo = platformRepo;
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<Platform> GetPlatformById(string id)
        {
            var platform = _platformRepo.GetPlatformById(id);
            if (platform != null)
            {
                return Ok(platform);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _platformRepo.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platform);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetPlatforms()
        {
            return Ok(_platformRepo.GetAllPlatforms());
        }
    }
}
