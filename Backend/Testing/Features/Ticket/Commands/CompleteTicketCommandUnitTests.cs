using Application.Common;
using Application.Exceptions;
using Application.Features.Ticket.Commands.CompleteTicketCommand;
using Domain.AggregateModels.Tickets;
using Moq;
using AutoFixture;
using DomainTicket = Domain.AggregateModels.Tickets.Ticket;
using UnitTesting.Common;

namespace UnitTesting.Features.Tickets.Commands
{
    public class CompleteTicketCommandUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITicketFinder> _ticketFinderMock = new();
        private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();
        private readonly Mock<IClock> _clockMock = new();

        public CompleteTicketCommandUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_complete_ticket_when_request_is_valid()
        {
            var now = DateTimeOffset.UtcNow;
            var userId = Guid.NewGuid();
            var ticket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.IN_PROGRESS, version: 7, assigneeId: userId);
            var request = new CompleteTicketCommand
            {
                TicketId = ticket.Id,
                UserId = userId,
                IsAdminUser = true,
                ExpectedVersion = 7
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(ticket.Id)).ReturnsAsync(ticket);
            _ticketRepositoryMock.Setup(x => x.UpdateAsync(ticket)).ReturnsAsync(ticket);
            _clockMock.SetupGet(x => x.UtcNow).Returns(now);

            var handler = new CompleteTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(TicketStatus.DONE, ticket.Status);
            Assert.Equal(8, ticket.Version);
            Assert.Equal(now, ticket.UpdatedAt);
            Assert.Equal("DONE", result.Items.Status);
        }

        [Fact]
        public async Task Handle_should_throw_when_ticket_does_not_exist()
        {
            var request = new CompleteTicketCommand
            {
                TicketId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsAdminUser = true,
                ExpectedVersion = 0
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(request.TicketId)).ReturnsAsync((DomainTicket?)null);

            var handler = new CompleteTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_user_is_not_authorized()
        {
            var ticket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.IN_PROGRESS, version: 1, assigneeId: Guid.NewGuid());
            var request = new CompleteTicketCommand
            {
                TicketId = ticket.Id,
                UserId = Guid.NewGuid(),
                IsAdminUser = false,
                ExpectedVersion = 1
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var handler = new CompleteTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<UnauthorizedActionException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_version_does_not_match()
        {
            var userId = Guid.NewGuid();
            var ticket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.IN_PROGRESS, version: 2, assigneeId: userId);
            var request = new CompleteTicketCommand
            {
                TicketId = ticket.Id,
                UserId = userId,
                IsAdminUser = true,
                ExpectedVersion = 1
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var handler = new CompleteTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<ConcurrencyConflictException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_ticket_is_not_in_progress()
        {
            var userId = Guid.NewGuid();
            var ticket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.DONE, version: 4, assigneeId: userId);
            var request = new CompleteTicketCommand
            {
                TicketId = ticket.Id,
                UserId = userId,
                IsAdminUser = true,
                ExpectedVersion = 4
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var handler = new CompleteTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<InvalidOperationApplicationException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
