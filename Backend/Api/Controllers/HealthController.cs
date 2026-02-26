using Infraestructure.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]  
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test([FromServices] AppDbContext db)
        {
            var users = await db.Users.CountAsync();
            var tickets = await db.Tickets.CountAsync();

            return Ok(new { users, tickets, serverTime = DateTimeOffset.UtcNow });
        }
    }
}
