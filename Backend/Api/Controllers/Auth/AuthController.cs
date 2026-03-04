using Application.Auth;
using Application.Common;
using Application.DTOs.AuthDto;
using Infraestructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly AppDbContext _db;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IClock _clock;

        public AuthController(AppDbContext db, IJwtTokenGenerator jwt, IClock clock)
        {
            _db = db;
            _jwt = jwt;
            _clock = clock;
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto req, CancellationToken ct)
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == req.Username, ct);

            if (user is null)
                return Unauthorized();

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Unauthorized();

            var role = user.Role.ToString();
            var token = _jwt.GenerateToken(user.Id, user.Username, role);

            var dto = new AuthUserDto(user.Id, user.Username, role);

            return Ok(new LoginResponseDto(token, dto, _clock.UtcNow));
        }
    }
}
