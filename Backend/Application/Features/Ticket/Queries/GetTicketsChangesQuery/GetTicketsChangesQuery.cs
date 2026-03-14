using Application.Features.Ticket.DTOs;
using MediatR;

namespace Application.Features.Ticket.Queries.GetTicketsChangesQuery
{
    public class GetTicketsChangesQuery: IRequest<IEnumerable<TicketDto>>
    {
        public string sinceIso {  get; init; }

        public GetTicketsChangesQuery() { }
    }
}
