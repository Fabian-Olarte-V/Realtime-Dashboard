using Application.Common;
using Application.Exceptions;
using Application.Features.Ticket.Commands.AssignTicketCommand;
using Domain.AggregateModels.Tickets;
using Moq;
using AutoFixture;
using DomainTicket = Domain.AggregateModels.Tickets.Ticket;
using UnitTesting.Common;

namespace UnitTesting.Features.Tickets.Commands
{
    public class AssingTicketCommandUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITicketFinder> _ticketFinderMock = new();
        private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();
        private readonly Mock<IClock> _clockMock = new();

        public AssingTicketCommandUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_assign_ticket_and_increment_version()
        {
            var now = DateTimeOffset.UtcNow;
            var ticket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.NEW, version: 2);
            var request = new AssignTicketCommand
            {
                TicketId = ticket.Id,
                UserId = Guid.NewGuid(),
                ExpectedVersion = 2
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(ticket.Id)).ReturnsAsync(ticket);
            _ticketRepositoryMock.Setup(x => x.UpdateAsync(ticket)).ReturnsAsync(ticket);
            _clockMock.SetupGet(x => x.UtcNow).Returns(now);

            var handler = new AssingTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(request.UserId, ticket.AssigneeId);
            Assert.Equal(TicketStatus.IN_PROGRESS, ticket.Status);
            Assert.Equal(3, ticket.Version);
            Assert.Equal(now, ticket.UpdatedAt);
            Assert.Equal("IN_PROGRESS", result.Items.Status);
            Assert.Equal(request.UserId, result.Items.AssigneeId);
        }

        [Fact]
        public async Task Handle_should_throw_when_ticket_does_not_exist()
        {
            var request = new AssignTicketCommand
            {
                TicketId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ExpectedVersion = 0
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(request.TicketId)).ReturnsAsync((DomainTicket?)null);

            var handler = new AssingTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_version_does_not_match()
        {
            var ticket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.NEW, version: 4);
            var request = new AssignTicketCommand
            {
                TicketId = ticket.Id,
                UserId = Guid.NewGuid(),
                ExpectedVersion = 3
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(ticket.Id)).ReturnsAsync(ticket);

            var handler = new AssingTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<ConcurrencyConflictException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_when_ticket_is_done_or_failed()
        {
            var doneTicket = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.DONE, version: 1);
            var request = new AssignTicketCommand
            {
                TicketId = doneTicket.Id,
                UserId = Guid.NewGuid(),
                ExpectedVersion = 1
            };

            _ticketFinderMock.Setup(x => x.GetTicketByIdAsync(doneTicket.Id)).ReturnsAsync(doneTicket);

            var handler = new AssingTicketCommandHandler(
                _ticketFinderMock.Object,
                _ticketRepositoryMock.Object,
                _clockMock.Object);

            await Assert.ThrowsAsync<InvalidOperationApplicationException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
