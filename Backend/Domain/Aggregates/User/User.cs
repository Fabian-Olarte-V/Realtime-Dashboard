using Domain.Common.Enums.User;

namespace Domain.AggregateModels.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }
}
