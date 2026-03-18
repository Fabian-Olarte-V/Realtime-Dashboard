using Application.Common;
using Application.Exceptions;
using Application.Features.Ticket.DTOs;
using Domain.AggregateModels.Tickets;
using MediatR;

namespace Application.Features.Ticket.Queries.GetTicketsChangesQuery
{
    public class GetTicketsChangesQueryHandler : IRequestHandler<GetTicketsChangesQuery, IEnumerable<TicketDto>>
    {
        private readonly ITicketFinder _ticketFinder;
        private readonly IClock _clock;

        public GetTicketsChangesQueryHandler(ITicketFinder ticketFinder, IClock clock)
        {
            _ticketFinder = ticketFinder;
            _clock = clock;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetTicketsChangesQuery request, CancellationToken cancellationToken)
        {
            if (!DateTimeOffset.TryParse(request.SinceIso, out var sinceDate))
                throw new InvalidRequestException("The provided 'since' value is not a valid ISO date.");

            var items = await _ticketFinder.GetAllTicketsAsync();
            var result = items
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
                ));

            return result;
        }
    }
}
