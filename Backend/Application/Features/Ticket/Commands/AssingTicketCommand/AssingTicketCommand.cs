using Application.Features.Ticket.DTOs;
using MediatR;

namespace Application.Features.Ticket.Commands.AssingTicketCommand
{
    public class AssingTicketCommand: IRequest<TicketMutationResponseDto>
    {
        public Guid TicketId { get; init; }
        public Guid UserId { get; init; }
        public int ExpectedVersion { get; init; }

        public AssingTicketCommand() { }
    }
}
