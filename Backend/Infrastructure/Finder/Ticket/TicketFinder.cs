using Domain.AggregateModels.Tickets;
using Infraestructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Finder.Tickets
{
    public class TicketFinder : ITicketFinder
    {
        private readonly AppDbContext _db;

        public TicketFinder (AppDbContext db)
        {
            _db = db;
        }

        public async Task<Ticket?> GetTicketByIdAsync(Guid id)
        {
            var result = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            return result;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            var result = await _db.Tickets.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(string? status, string? querySearch, string? sort)
        {
            var query = _db.Tickets.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && status is not "ALL")
                query = query.Where(t => t.Status.ToString() == status);


            if (!string.IsNullOrWhiteSpace(querySearch))
            {
                var finalQuery = querySearch.Trim();
                query = query.Where(t => t.Title.Contains(finalQuery) || t.Description.Contains(finalQuery));
            }

            query = (sort?.ToLowerInvariant()) switch
            {
                "createdat" => query.OrderBy(t => t.CreatedAt),
                "deadlineat" => query.OrderBy(t => t.DeadlineAt),
                _ => query.OrderBy(t => t.UpdatedAt),
            };

            var items = await query.ToListAsync();
            return items;
        }
    }
}
