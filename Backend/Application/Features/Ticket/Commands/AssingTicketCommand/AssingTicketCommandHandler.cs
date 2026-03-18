using Application.Common;
using Application.Exceptions;
using Application.Features.Ticket.DTOs;
using Domain.AggregateModels.Tickets;
using MediatR;


namespace Application.Features.Ticket.Commands.AssingTicketCommand
{
    public class AssingTicketCommandHandler : IRequestHandler<AssingTicketCommand, TicketMutationResponseDto>
    {
        private readonly ITicketFinder _ticketFinder;
        private readonly ITicketRepository _ticketRepository;
        private readonly IClock _clock;

        public AssingTicketCommandHandler(ITicketFinder ticketFinder, ITicketRepository ticketRepository, IClock clock)
        {
            _ticketFinder = ticketFinder;
            _ticketRepository = ticketRepository;
            _clock = clock;
        }


        public async Task<TicketMutationResponseDto> Handle(AssingTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketFinder.GetTicketByIdAsync(request.TicketId);
            if (ticket is null) throw new EntityNotFoundException("Ticket", request.TicketId);

            //optimistic concurrency 
            if (ticket.Version != request.ExpectedVersion)
                throw new ConcurrencyConflictException("The ticket has been modified by another user. Please refresh and try again.");

            if (ticket.Status == TicketStatus.DONE || ticket.Status == TicketStatus.FAILED)
                throw new InvalidOperationApplicationException("Cannot assign a ticket that is already completed or failed.");


            ticket.AssigneeId = request.UserId;
            ticket.Status = TicketStatus.IN_PROGRESS;
            ticket.Version += 1;
            ticket.UpdatedAt = _clock.UtcNow;

            var ticketUpdated = await _ticketRepository.UpdateAsync(ticket);

            var ticketResponse = new TicketDto(
                ticketUpdated.Id,
                ticketUpdated.Title,
                ticketUpdated.Description,
                ticketUpdated.Status.ToString(),
                ticketUpdated.AssigneeId,
                ticketUpdated.Version,
                ticketUpdated.CreatedAt,
                ticketUpdated.UpdatedAt,
                ticketUpdated.DeadlineAt
            );

            return new TicketMutationResponseDto(ticketResponse);
        }
    }
}
