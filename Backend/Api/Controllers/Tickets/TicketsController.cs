using Api.Common.Auth;
using Application.Features.Ticket.Commands.AssignTicketCommand;
using Application.Features.Ticket.Commands.CompleteTicketCommand;
using Application.Features.Ticket.DTOs;
using Application.Features.Ticket.Queries.GetFilteredTicketsQuery;
using Application.Features.Ticket.Queries.GetTicketsChangesQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Tickets
{
    [ApiController]
    [Route("api/tickets")]
    [Authorize(AuthPolicies.AgentOrAdmin)]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetTickets([FromQuery] GetFilteredTicketsQuery req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }

        [HttpGet("changes")]
        public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetChanges([FromQuery] GetTicketsChangesQuery req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }

        [HttpPut("{id:guid}/assign")]
        public async Task<ActionResult> AssignTicket(Guid id, [FromBody] TicketMutationRequestDto req)
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var request = new AssignTicketCommand()
            {
                TicketId = id,
                UserId = userId,
                ExpectedVersion = req.ExpectedVersion
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id:guid}/complete")]   
        public async Task<ActionResult> CompleteTicket(Guid id, [FromBody] TicketMutationRequestDto req)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsAdmin();

            var request = new CompleteTicketCommand()
            {
                TicketId = id,
                UserId = userId,
                IsAdminUser = isAdmin,
                ExpectedVersion = req.ExpectedVersion
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
