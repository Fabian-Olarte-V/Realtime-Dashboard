using Api.Common.Auth;
using Application.Features.Ticket.Commands.AssingTicketCommand;
using Application.Features.Ticket.DTOs;
using Application.Features.Ticket.Queries.GetFilteredTicketsQuery;
using Application.Features.Ticket.Queries.GetTicketsChangesQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Tickets
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetTickets([FromQuery] string? status,
            [FromQuery] Guid? assigneeId,
            [FromQuery] string? q,
            [FromQuery] string? sort,
            [FromQuery] string? dir
        ){
            var request = new GetFilteredTicketsQuery() { 
                AssigneeId = assigneeId, 
                Dir = dir, 
                Query = q, 
                Sort= sort, 
                Status = status 
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("changes")]
        public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetChanges([FromQuery] string sinceIso)
        {
            var request = new GetTicketsChangesQuery() { sinceIso = sinceIso };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("{id:guid}/assing")]
        public async Task<ActionResult> AssignTicket(Guid id, [FromBody] int ExpectedVersion)
        {
            //User is not the class but is a property of the ControllerBase that represents the currently authenticated user
            //That's why I can use extension methods of ClaimsPrincipal.
            var userId = User.GetUserId();                
            if (userId == Guid.Empty) return Unauthorized();

            var request = new AssingTicketCommand() { TicketId = id, UserId = userId, ExpectedVersion = ExpectedVersion }; 
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<ActionResult> CompleteTicket(Guid id, [FromBody] int ExpectedVersion)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsAdmin();

            var request = new AssingTicketCommand() { TicketId = id, UserId = userId, ExpectedVersion = ExpectedVersion };
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
