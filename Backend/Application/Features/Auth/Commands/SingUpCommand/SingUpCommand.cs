using Application.Features.Auth.DTOs;
using Domain.Common.Enums.Users;
using FluentValidation;
using MediatR;

namespace Application.Features.Auth.Commands.SingUpCommand
{
    public class SingUpCommand : IRequest<AuthResponseDto>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }

    public class SingUpCommandValidator : AbstractValidator<SingUpCommand>
    {
        public SingUpCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(BeAValidRole)
                .WithMessage("Role must be one of: ADMIN, AGENT.");
        }

        private static bool BeAValidRole(string role)
        {
            return Enum.TryParse<UserRole>(role, true, out _);
        }
    }
}
