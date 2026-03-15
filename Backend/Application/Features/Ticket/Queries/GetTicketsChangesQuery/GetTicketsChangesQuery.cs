using Application.Features.Ticket.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Features.Ticket.Queries.GetTicketsChangesQuery
{
    public class GetTicketsChangesQuery : IRequest<IEnumerable<TicketDto>>
    {
        public required string SinceIso { get; init; }
    }

    public class GetTicketsChangesQueryValidator : AbstractValidator<GetTicketsChangesQuery>
    {
        public GetTicketsChangesQueryValidator()
        {
            RuleFor(x => x.SinceIso)
                .NotEmpty()
                .Must(sinceIso => DateTimeOffset.TryParse(sinceIso, out _))
                .WithMessage("sinceIso must be a valid ISO date.");
        }
    }
}
