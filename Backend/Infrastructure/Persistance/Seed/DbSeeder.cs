using Domain.AggregateModels.Users;
using Domain.AggregateModels.Tickets;
using Domain.Common.Enums.User;
using Infraestructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if(!await context.Users.AnyAsync())
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin User",
                    Username = "admin",
                    Email = "admin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.ADMIN
                };

                var agent = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Agent User",
                    Username = "agent",
                    Email = "agent@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("agent123"),
                    Role = UserRole.AGENT
                };

                context.Users.AddRange(admin, agent);
                await context.SaveChangesAsync();
            }

            if (!await context.Tickets.AnyAsync())
            {
                var users = await context.Users.ToListAsync();
                var agent = users.First(u => u.Role == UserRole.AGENT);

                var now = DateTimeOffset.UtcNow;

                var tickets = Enumerable.Range(1, 500).Select(i =>
                {
                    var status = (TicketStatus)(i % 4);
                    var deadline = now.AddMinutes((i % 60) - 30);

                    return new Ticket
                    {
                        Id = Guid.NewGuid(),
                        Title = $"Ticket {i}",
                        Description = $"Seeded ticket {i}",
                        Status = status,
                        Priority = (i % 5) + 1,
                        AssigneeId = status == TicketStatus.IN_PROGRESS ? agent.Id : null,
                        DeadlineAt = deadline,
                        CreatedAt = now.AddMinutes(-i),
                        UpdatedAt = now.AddMinutes(-i / 2),
                        Version = 1,
                        FailReason = status == TicketStatus.FAILED ? "DEADLINE_EXCEEDED" : null
                    };
                }).ToList();

                context.Tickets.AddRange(tickets);
                await context.SaveChangesAsync();
            }
        }
    }
}
