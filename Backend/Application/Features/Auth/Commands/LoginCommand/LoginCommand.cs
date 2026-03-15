using Application.Features.Auth.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommand: IRequest<LoginResponseDto>
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().MinimumLength(2).WithMessage("AAAAAAAAAAA");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}
