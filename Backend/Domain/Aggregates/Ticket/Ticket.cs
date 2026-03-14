using Domain.AggregateModels.Users;

namespace Domain.AggregateModels.Tickets
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public TicketStatus Status { get; set; }
        public int Priority { get; set; }

        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        public DateTimeOffset DeadlineAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public int Version { get; set; }
        public string? FailReason { get; set; }
    }
}
