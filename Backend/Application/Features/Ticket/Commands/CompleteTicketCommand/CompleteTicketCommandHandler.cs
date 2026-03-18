using Application.Common;
using Application.Exceptions;
using Application.Features.Ticket.DTOs;
using Domain.AggregateModels.Tickets;
using MediatR;

namespace Application.Features.Ticket.Commands.CompleteTicketCommand
{
    public class CompleteTicketCommandHandler : IRequestHandler<CompleteTicketCommand, TicketMutationResponseDto>
    {
        private readonly ITicketFinder _ticketFinder;
        private readonly ITicketRepository _ticketRepository;
        private readonly IClock _clock;

        public CompleteTicketCommandHandler(ITicketFinder ticketFinder, ITicketRepository ticketRepository, IClock clock)
        {
            _ticketFinder = ticketFinder;
            _ticketRepository = ticketRepository;
            _clock = clock;
        }

        public async Task<TicketMutationResponseDto> Handle(CompleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketFinder.GetTicketByIdAsync(request.TicketId);
            if (ticket is null) 
                throw new EntityNotFoundException("Ticket", request.TicketId);


            if (!request.IsAdminUser || ticket.AssigneeId != request.UserId) 
                throw new UnauthorizedActionException();


            if (ticket.Version != request.ExpectedVersion)
                throw new ConcurrencyConflictException("The ticket has been modified by another user. Please refresh and try again.");

            if (ticket.Status != TicketStatus.IN_PROGRESS)
                throw new InvalidOperationApplicationException("Cannot complete a ticket that is already completed or failed.");


            ticket.Status = TicketStatus.DONE;
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
