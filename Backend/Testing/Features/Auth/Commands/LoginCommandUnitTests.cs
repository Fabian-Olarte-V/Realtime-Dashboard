using Application.Common;
using Application.Exceptions;
using Application.Features.Auth.Commands.LoginCommand;
using Domain.AggregateModels.Users;
using Domain.Common.Enums.Users;
using Moq;
using AutoFixture;
using DomainUser = Domain.AggregateModels.Users.User;
using UnitTesting.Common;

namespace UnitTesting.Features.Auth.Commands
{
    public class LoginCommandUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserFinder> _userFinderMock = new();
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();

        public LoginCommandUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_return_jwt_when_credentials_are_valid()
        {
            var plainPassword = _fixture.Create<string>();
            var user = DomainTestData.CreateUser(_fixture.Create<string>(), plainPassword, UserRole.ADMIN);
            var token = _fixture.Create<string>();

            var request = new LoginCommand
            {
                Username = user.Username,
                Password = plainPassword
            };

            _userFinderMock
                .Setup(x => x.FindByUsernameAsync(user.Username))
                .ReturnsAsync(user);

            _jwtTokenGeneratorMock
                .Setup(x => x.GenerateToken(user.Id, user.Username, user.Role.ToString()))
                .Returns(token);

            var handler = new LoginCommandHandler(_userFinderMock.Object, _jwtTokenGeneratorMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(token, result.Token);
            Assert.Equal(user.Id, result.User.Id);
            Assert.Equal(user.Username, result.User.Username);
            Assert.Equal("ADMIN", result.User.Role);
        }

        [Fact]
        public async Task Handle_should_throw_when_user_does_not_exist()
        {
            var request = new LoginCommand
            {
                Username = _fixture.Create<string>(),
                Password = _fixture.Create<string>()
            };

            _userFinderMock
                .Setup(x => x.FindByUsernameAsync(request.Username))
                .ReturnsAsync((DomainUser?)null);

            var handler = new LoginCommandHandler(_userFinderMock.Object, _jwtTokenGeneratorMock.Object);

            await Assert.ThrowsAsync<InvalidCredentialsException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_password_is_invalid()
        {
            var user = DomainTestData.CreateUser(_fixture.Create<string>(), "correct-pass", UserRole.ADMIN);
            var request = new LoginCommand
            {
                Username = user.Username,
                Password = _fixture.Create<string>()
            };

            _userFinderMock
                .Setup(x => x.FindByUsernameAsync(request.Username))
                .ReturnsAsync(user);

            var handler = new LoginCommandHandler(_userFinderMock.Object, _jwtTokenGeneratorMock.Object);

            await Assert.ThrowsAsync<InvalidCredentialsException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
