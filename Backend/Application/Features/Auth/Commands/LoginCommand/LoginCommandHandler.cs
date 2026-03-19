using Application.Common;
using Application.Exceptions;
using Application.Features.Auth.DTOs;
using Domain.AggregateModels.Users;
using MediatR;

namespace Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUserFinder _userFinder;
        private readonly IJwtTokenGenerator _jwt;

        public LoginCommandHandler(IUserFinder userFinder, IJwtTokenGenerator jwt)
        {
            _userFinder = userFinder;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userFinder.FindByUsernameAsync(request.Username);
            if (user is null) throw new InvalidCredentialsException();

            var passwordHashValidation = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordHashValidation) throw new InvalidCredentialsException();

            var role = user.Role.ToString();
            var token = _jwt.GenerateToken(user.Id, user.Username, role);

            var userDto = new AuthUserDto(user.Id, user.Username, role);
            return new AuthResponseDto(token, userDto);
        }
    }
}
