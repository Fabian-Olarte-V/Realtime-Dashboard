using Application.Common;
using Application.Features.Ticket.Queries.GetFilteredTicketsQuery;
using Domain.AggregateModels.Tickets;
using Moq;
using AutoFixture;
using UnitTesting.Common;

namespace UnitTesting.Features.Tickets.Queries
{
    public class GetFilteredTicketsQueryUnitTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITicketFinder> _ticketFinderMock = new();
        private readonly Mock<IClock> _clockMock = new();

        public GetFilteredTicketsQueryUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_should_map_filtered_tickets_to_dto()
        {
            var assigneeId = Guid.NewGuid();
            var request = new GetFilteredTicketsQuery
            {
                Status = "IN_PROGRESS",
                Query = "printer",
                Sort = "updatedAt",
            };

            var tickets = new[]
            {
                DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.IN_PROGRESS, 3, assigneeId),
                DomainTestData.CreateTicket(_fixture.Create<string>(), TicketStatus.NEW, 1)
            };

            _ticketFinderMock
                .Setup(x => x.GetFilteredTicketsAsync(request.Status, request.Query, request.Sort))
                .ReturnsAsync(tickets);

            var handler = new GetFilteredTicketsQueryHandler(_ticketFinderMock.Object, _clockMock.Object);

            var result = (await handler.Handle(request, CancellationToken.None)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(tickets[0].Id, result[0].Id);
            Assert.Equal("IN_PROGRESS", result[0].Status);
            Assert.Equal(tickets[1].Id, result[1].Id);
            Assert.Equal("NEW", result[1].Status);
        }
    }
}
