using Application.Features.Users.Queries.GetAllUsersQuery;
using Domain.AggregateModels.Users;
using Domain.Common.Enums.Users;
using Moq;
using AutoFixture;
using DomainUser = Domain.AggregateModels.Users.User;
using UnitTesting.Common;

namespace UnitTesting.Features.Users.Queries
{
    public class GetAllUsersQueryUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserFinder> _userFinderMock = new();

        public GetAllUsersQueryUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_map_all_users_to_dto()
        {
            var users = new[]
            {
                DomainTestData.CreateUser(_fixture.Create<string>(), "admin123", UserRole.ADMIN),
                DomainTestData.CreateUser(_fixture.Create<string>(), "agent123", UserRole.AGENT)
            };

            _userFinderMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(users);

            var handler = new GetAllUsersQueryHandler(_userFinderMock.Object);

            var result = (await handler.Handle(new GetAllUsersQuery(), CancellationToken.None)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(users[0].Id, result[0].Id);
            Assert.Equal(users[0].Username, result[0].Username);
            Assert.Equal("ADMIN", result[0].Role);
            Assert.Equal(users[1].Username, result[1].Username);
            Assert.Equal("AGENT", result[1].Role);
        }
    }
}
