using Application.Features.Ticket.DTOs;
using MediatR;

namespace Application.Features.Ticket.Queries.GetFilteredTicketsQuery
{
    public class GetFilteredTicketsQuery: IRequest<IEnumerable<TicketDto>>
    {
        public string? Status { get; init; }

        public string? Query { get; init; }

        public string? Sort { get; init; }
    }
}
