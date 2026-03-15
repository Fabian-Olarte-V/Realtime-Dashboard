using Application.Features.User.DTOs;
using Application.Features.User.Queries.GetAllUsersQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }
    }
}
