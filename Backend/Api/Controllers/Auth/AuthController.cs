using Application.Features.Auth.Commands.LoginCommand;
using Application.Features.Auth.Commands.SingUpCommand;
using Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginCommand req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDto>> SingUp(SignUpCommand req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }
    }
}
