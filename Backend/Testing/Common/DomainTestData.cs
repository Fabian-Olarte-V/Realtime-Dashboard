using Domain.AggregateModels.Tickets;
using Domain.Common.Enums.Users;
using DomainTicket = Domain.AggregateModels.Tickets.Ticket;
using DomainUser = Domain.AggregateModels.Users.User;

namespace UnitTesting.Common
{
    internal static class DomainTestData
    {
        public static DomainUser CreateUser(
            string username,
            string plainPassword,
            UserRole role)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            return new DomainUser(username, passwordHash, role);
        }

        public static DomainTicket CreateTicket(
            string title,
            TicketStatus status,
            int version,
            Guid? assigneeId = null,
            DateTimeOffset? createdAt = null,
            DateTimeOffset? updatedAt = null,
            DateTimeOffset? deadlineAt = null)
        {
            var now = DateTimeOffset.UtcNow;

            return new DomainTicket
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = $"{title} description",
                Status = status,
                AssigneeId = assigneeId,
                Version = version,
                CreatedAt = createdAt ?? now.AddHours(-1),
                UpdatedAt = updatedAt ?? now.AddMinutes(-30),
                DeadlineAt = deadlineAt ?? now.AddHours(1)
            };
        }
    }
}
