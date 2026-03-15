using Application.Features.Ticket.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Features.Ticket.Commands.AssingTicketCommand
{
    public class AssingTicketCommand : IRequest<TicketMutationResponseDto>
    {
        public Guid TicketId { get; init; }
        public Guid UserId { get; init; }
        public int ExpectedVersion { get; init; }
    }

    public class AssingTicketCommandValidator : AbstractValidator<AssingTicketCommand>
    {
        public AssingTicketCommandValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ExpectedVersion)
                .GreaterThanOrEqualTo(0);
        }
    }
}
