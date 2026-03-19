namespace Domain.AggregateModels.Users
{
    public interface IUserFinder
    {
        Task<User?> FindByIdAsync(Guid id);
        Task<User?> FindByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
