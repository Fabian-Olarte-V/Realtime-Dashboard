using Infraestructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "AdminOnly")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers(CancellationToken ct)
        {
            var users = await _db.Users
                .AsNoTracking()
                .OrderBy(u => u.Username)
                .Select(u => new UserDto(u.Id, u.Username, u.Role.ToString()))
                .ToListAsync(ct);

            return Ok(users);
        }
    }
}
