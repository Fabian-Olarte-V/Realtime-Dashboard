using Domain.Common.Enums.Users;

namespace Domain.AggregateModels.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public User() { }

        public User(string username, string passwordHash, UserRole role)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
