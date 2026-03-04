using Application.Common;
using Application.DTOs.Ticket;
using Infraestructure.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Enums.Ticket;
using Api.Auth;

namespace Api.Controllers.Tickets
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IClock _clock;

        public TicketsController(AppDbContext db, IClock clock)
        {
            _db = db;
            _clock = clock;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetTickets(
            [FromQuery] string? status,
            [FromQuery] Guid? assigneeId,
            [FromQuery] string? q,
            [FromQuery] string? sort,
            [FromQuery] string? dir,
            CancellationToken ct)
        {
            var query = _db.Tickets.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status.ToString() == status);

            if (assigneeId is not null)
                query = query.Where(t => t.AssigneeId == assigneeId);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var finalQuery = q.Trim();
                query = query.Where(t => t.Title.Contains(finalQuery) || t.Description.Contains(finalQuery));
            }


            var desc = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

            query = (sort?.ToLowerInvariant()) switch
            {
                "createdat" => desc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                "deadlineat" => desc ? query.OrderByDescending(t => t.DeadlineAt) : query.OrderBy(t => t.DeadlineAt),
                _ => desc ? query.OrderByDescending(t => t.UpdatedAt) : query.OrderBy(t => t.UpdatedAt),
            };

            var items = await query
                .Select(t => new TicketDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status.ToString(),
                    t.AssigneeId,
                    t.Version,
                    t.CreatedAt,
                    t.UpdatedAt,
                    t.DeadlineAt
                ))
                .ToListAsync(ct);


            return Ok(items);
        }

        [HttpGet("changes")]
        public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetChanges([FromQuery] string since, CancellationToken ct)
        {
            if (!DateTimeOffset.TryParse(since, out var sinceDate))
                return BadRequest("Invalid date format for 'since' parameter.");

            var items = await _db.Tickets
                .AsNoTracking()
                .Where(t => t.UpdatedAt > sinceDate)
                .OrderBy(t => t.UpdatedAt)
                .Select(t => new TicketDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status.ToString(),
                    t.AssigneeId,
                    t.Version,
                    t.CreatedAt,
                    t.UpdatedAt,
                    t.DeadlineAt
                ))
                .ToListAsync(ct);


            return Ok(items);
        }

        [HttpPost("{id:guid}/assing")]
        [Authorize(Policy = "AgentOrAdmin")]
        public async Task<ActionResult> AssignTicket(
            Guid id, 
            [FromBody] AssingTicketRequestDto req, 
            CancellationToken ct)
        {
            //User is not the class but is a property of the ControllerBase that represents the currently authenticated user
            //That's why I can use extension methods of ClaimsPrincipal.
            var userId = User.GetUserId();                
            if (userId == Guid.Empty) return Unauthorized();

            var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (ticket is null) return NotFound();

            //optimistic concurrency 
            if(ticket.Version != req.ExpectedVersion)
                return Conflict("The ticket has been modified by another user. Please refresh and try again.");

            if(ticket.Status == TicketStatus.DONE || ticket.Status == TicketStatus.FAILED)
                return BadRequest("Cannot assign a ticket that is already completed or failed.");


            ticket.AssigneeId = userId;
            ticket.Status = TicketStatus.IN_PROGRESS;
            ticket.Version += 1;
            ticket.UpdatedAt = _clock.UtcNow;

            await _db.SaveChangesAsync(ct);

            var ticketResponse = new TicketDto(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status.ToString(),
                ticket.AssigneeId,
                ticket.Version,
                ticket.CreatedAt,
                ticket.UpdatedAt,
                ticket.DeadlineAt
            );

            return Ok(new TicketMutationResponseDto(ticketResponse, _clock.UtcNow));
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<ActionResult> CompleteTicket(
            Guid id,
            [FromBody] AssingTicketRequestDto req,
            CancellationToken ct)
        {
            var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (ticket is null) return NotFound();

            var userId = User.GetUserId();
            var isAdmin = User.IsAdmin();

            if (!isAdmin || ticket.AssigneeId != userId) 
                return Forbid();


            //optimistic concurrency 
            if (ticket.Version != req.ExpectedVersion)
                return Conflict("The ticket has been modified by another user. Please refresh and try again.");

            if (ticket.Status != TicketStatus.IN_PROGRESS)
                return BadRequest("Cannot complete a ticket that is already completed or failed.");


            ticket.Status = TicketStatus.DONE;
            ticket.Version += 1;
            ticket.UpdatedAt = _clock.UtcNow;

            await _db.SaveChangesAsync(ct);

            var ticketResponse = new TicketDto(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status.ToString(),
                ticket.AssigneeId,
                ticket.Version,
                ticket.CreatedAt,
                ticket.UpdatedAt,
                ticket.DeadlineAt
            );

            return Ok(new TicketMutationResponseDto(ticketResponse, _clock.UtcNow));
        }
    }
}
