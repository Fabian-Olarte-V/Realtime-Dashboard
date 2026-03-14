using Domain.AggregateModels.Users;
using Infraestructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Finder.Users
{
    public class UserFinder : IUserFinder
    {
        private readonly AppDbContext _db;

        public UserFinder(AppDbContext db)
        {
            _db = db;
        }


        public async Task<User> FindByIdAsync(Guid id)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Id  == id);
            
            return user;
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }
    }
}
