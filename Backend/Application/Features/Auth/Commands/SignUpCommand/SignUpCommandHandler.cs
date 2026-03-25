using Application.Common;
using Application.Exceptions;
using Application.Features.Auth.DTOs;
using Domain.AggregateModels.Users;
using MediatR;

namespace Application.Features.Auth.Commands.SignUpCommand
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthResponseDto>
    {
        private readonly IUserFinder _userFinder;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwt;

        public SignUpCommandHandler(IUserFinder userFinder, IUserRepository userRepository, IJwtTokenGenerator jwt)
        {
            _userFinder = userFinder;
            _userRepository = userRepository;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userFinder.FindByUsernameAsync(request.Username);
            if (existingUser is not null)
            {
                throw new InvalidRequestException("Username already exists.");
            }

            if (!Enum.TryParse<Domain.Common.Enums.Users.UserRole>(request.Role, true, out var role))
            {
                throw new InvalidRequestException("Invalid role.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User(request.Username, passwordHash, role);

            var savedUser = await _userRepository.AddAsync(newUser);
            var savedRole = savedUser.Role.ToString();
            var token = _jwt.GenerateToken(savedUser.Id, savedUser.Username, savedRole);
            var userDto = new AuthUserDto(savedUser.Id, savedUser.Username, savedRole);

            return new AuthResponseDto(token, userDto);
        }
    }
}
