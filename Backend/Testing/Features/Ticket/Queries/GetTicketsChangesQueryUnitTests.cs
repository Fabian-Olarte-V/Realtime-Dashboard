using Application.Common;
using Application.Exceptions;
using Application.Features.Ticket.Queries.GetTicketsChangesQuery;
using Domain.AggregateModels.Tickets;
using Moq;
using AutoFixture;
using DomainTicket = Domain.AggregateModels.Tickets.Ticket;
using UnitTesting.Common;

namespace UnitTesting.Features.Tickets.Queries
{
    public class GetTicketsChangesQueryUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITicketFinder> _ticketFinderMock = new();
        private readonly Mock<IClock> _clockMock = new();

        public GetTicketsChangesQueryUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_return_only_items_updated_after_since_ordered_by_updated_at()
        {
            var since = new DateTimeOffset(2026, 03, 18, 10, 00, 00, TimeSpan.Zero);
            var older = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.NEW, 1, updatedAt: since.AddMinutes(-5));
            var newer = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.IN_PROGRESS, 1, updatedAt: since.AddMinutes(10));
            var latest = DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.DONE, 1, updatedAt: since.AddMinutes(20));

            _ticketFinderMock
                .Setup(x => x.GetAllTicketsAsync())
                .ReturnsAsync(new[] { latest, older, newer });

            var handler = new GetTicketsChangesQueryHandler(_ticketFinderMock.Object, _clockMock.Object);

            var result = (await handler.Handle(
                new GetTicketsChangesQuery { SinceIso = since.ToString("O") },
                CancellationToken.None)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(newer.Id, result[0].Id);
            Assert.Equal(latest.Id, result[1].Id);
        }

        [Fact]
        public async Task Handle_should_throw_when_since_iso_is_invalid()
        {
            var handler = new GetTicketsChangesQueryHandler(_ticketFinderMock.Object, _clockMock.Object);

            await Assert.ThrowsAsync<InvalidRequestException>(() => handler.Handle(
                new GetTicketsChangesQuery { SinceIso = "not-a-date" },
                CancellationToken.None));
        }
    }
}
