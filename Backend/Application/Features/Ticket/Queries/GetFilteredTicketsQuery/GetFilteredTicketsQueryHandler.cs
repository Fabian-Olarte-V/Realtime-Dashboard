using Application.Common;
using Application.Features.Ticket.DTOs;
using Domain.AggregateModels.Tickets;
using MediatR;

namespace Application.Features.Ticket.Queries.GetFilteredTicketsQuery
{
    public class GetFilteredTicketsQueryHandler : IRequestHandler<GetFilteredTicketsQuery, IEnumerable<TicketDto>>
    {
        private readonly ITicketFinder _ticketFinder;
        private readonly IClock _clock;

        public GetFilteredTicketsQueryHandler(ITicketFinder ticketFinder, IClock clock)
        {
            _ticketFinder = ticketFinder;
            _clock = clock;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetFilteredTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketFinder.GetFilteredTicketsAsync(request.AssigneeId, 
                request.Status, request.Query, request.Sort, request.Dir);

            var result = tickets.Select(t => 
                new TicketDto(
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
            .ToList();

            return result;
        }
    }
}
