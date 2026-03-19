using Application.Common;
using Application.Exceptions;
using Application.Features.Auth.Commands.SingUpCommand;
using Domain.AggregateModels.Users;
using Domain.Common.Enums.Users;
using Moq;
using AutoFixture;
using DomainUser = Domain.AggregateModels.Users.User;
using UnitTesting.Common;

namespace UnitTesting.Features.Auth.Commands
{
    public class SingUpCommandUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserFinder> _userFinderMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();

        public SingUpCommandUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_create_user_and_return_jwt_when_request_is_valid()
        {
            var request = new SingUpCommand
            {
                Username = _fixture.Create<string>(),
                Password = _fixture.Create<string>(),
                Role = "ADMIN"
            };

            DomainUser? persistedUser = null;
            var token = _fixture.Create<string>();

            _userFinderMock
                .Setup(x => x.FindByUsernameAsync(request.Username))
                .ReturnsAsync((DomainUser?)null);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DomainUser>()))
                .Callback<DomainUser>(user => persistedUser = user)
                .ReturnsAsync((DomainUser user) => user);

            _jwtTokenGeneratorMock
                .Setup(x => x.GenerateToken(It.IsAny<Guid>(), request.Username, "ADMIN"))
                .Returns(token);

            var handler = new SingUpCommandHandler(
                _userFinderMock.Object,
                _userRepositoryMock.Object,
                _jwtTokenGeneratorMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(persistedUser);
            Assert.Equal(request.Username, persistedUser!.Username);
            Assert.True(BCrypt.Net.BCrypt.Verify(request.Password, persistedUser.PasswordHash));
            Assert.Equal(UserRole.ADMIN, persistedUser.Role);
            Assert.Equal(token, result.Token);
            Assert.Equal(request.Username, result.User.Username);
            Assert.Equal("ADMIN", result.User.Role);
        }

        [Fact]
        public async Task Handle_should_throw_when_username_already_exists()
        {
            var request = new SingUpCommand
            {
                Username = _fixture.Create<string>(),
                Password = _fixture.Create<string>(),
                Role = "AGENT"
            };

            var existingUser = DomainTestData.CreateUser(request.Username, "old-pass", UserRole.AGENT);

            _userFinderMock
                .Setup(x => x.FindByUsernameAsync(request.Username))
                .ReturnsAsync(existingUser);

            var handler = new SingUpCommandHandler(
                _userFinderMock.Object,
                _userRepositoryMock.Object,
                _jwtTokenGeneratorMock.Object);

            await Assert.ThrowsAsync<InvalidRequestException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_role_is_invalid()
        {
            var request = new SingUpCommand
            {
                Username = _fixture.Create<string>(),
                Password = _fixture.Create<string>(),
                Role = "ROOT"
            };

            _userFinderMock
                .Setup(x => x.FindByUsernameAsync(request.Username))
                .ReturnsAsync((DomainUser?)null);

            var handler = new SingUpCommandHandler(
                _userFinderMock.Object,
                _userRepositoryMock.Object,
                _jwtTokenGeneratorMock.Object);

            await Assert.ThrowsAsync<InvalidRequestException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
