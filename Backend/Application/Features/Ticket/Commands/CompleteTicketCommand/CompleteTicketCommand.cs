using Application.Features.Ticket.DTOs;
using MediatR;

namespace Application.Features.Ticket.Commands.CompleteTicketCommand
{
    public class CompleteTicketCommand : IRequest<TicketMutationResponseDto>
    {
        public Guid TicketId { get; init; }
        public Guid UserId { get; init; }
        public bool IsAdminUser { get; init; }
        public int ExpectedVersion { get; init; }

        public CompleteTicketCommand() { }
    }
}
