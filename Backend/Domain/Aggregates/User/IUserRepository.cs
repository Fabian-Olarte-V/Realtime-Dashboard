namespace Domain.AggregateModels.Users
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
    }
}
