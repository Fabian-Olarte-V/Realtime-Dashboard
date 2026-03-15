using Application.Common;
using Application.Features.Auth.DTOs;
using Domain.AggregateModels.Users;
using MediatR;

namespace Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserFinder _userFinder;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IClock _clock;

        public LoginCommandHandler(IUserFinder userFinder, IJwtTokenGenerator jwt, IClock clock)
        {
            _userFinder = userFinder;
            _jwt = jwt;
            _clock = clock;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userFinder.FindByUsernameAsync(request.Username);
            if (user is null) throw new ArgumentNullException(nameof(user));

            var passwordHashValidation = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordHashValidation) throw new ArgumentNullException(nameof(user));

            var role = user.Role.ToString();
            var token = _jwt.GenerateToken(user.Id, user.Username, role);

            var dto = new AuthUserDto(user.Id, user.Username, role);
            return new LoginResponseDto(token, dto, _clock.UtcNow);
        }
    }
}
