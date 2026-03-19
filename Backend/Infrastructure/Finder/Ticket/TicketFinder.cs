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

        public async Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(Guid? assigneeId, string? status, 
            string? querySearch, string? sort, string? dir)
        {
            var query = _db.Tickets.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status.ToString() == status);

            if (assigneeId is not null)
                query = query.Where(t => t.AssigneeId == assigneeId);

            if (!string.IsNullOrWhiteSpace(querySearch))
            {
                var finalQuery = querySearch.Trim();
                query = query.Where(t => t.Title.Contains(finalQuery) || t.Description.Contains(finalQuery));
            }


            var desc = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);
            query = (sort?.ToLowerInvariant()) switch
            {
                "createdat" => desc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                "deadlineat" => desc ? query.OrderByDescending(t => t.DeadlineAt) : query.OrderBy(t => t.DeadlineAt),
                _ => desc ? query.OrderByDescending(t => t.UpdatedAt) : query.OrderBy(t => t.UpdatedAt),
            };

            var items = await query.ToListAsync();
            return items;
        }
    }
}
