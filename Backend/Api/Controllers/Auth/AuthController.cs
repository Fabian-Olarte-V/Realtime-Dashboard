using Application.Features.Auth.DTOs;
using Application.Features.Auth.Queries.LoginQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginQuery req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }
    }
}
