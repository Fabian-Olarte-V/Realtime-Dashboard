using Application.DTOs.User;
using Infraestructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private const string CacheKey = "users:all:v1";
        private readonly AppDbContext _db;
        private readonly IMemoryCache _cache;

        public UserController(AppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> Get(CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<UserDto>? cached) && cached is not null)         
                return Ok(cached);

            var users = await _db.Users
                .AsNoTracking()
                .OrderBy(x => x.Username)
                .Select(x => new UserDto(x.Id, x.Username, x.Role.ToString()))
                .ToListAsync(ct);

            _cache.Set(CacheKey, users, TimeSpan.FromMinutes(5));

            return Ok(users);
        }
    }
}
