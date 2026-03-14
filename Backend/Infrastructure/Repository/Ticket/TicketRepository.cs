using Domain.AggregateModels.Tickets;
using Infraestructure.Persistance;

namespace Infrastructure.Repository.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _db;

        public TicketRepository (AppDbContext db)
        {
            _db = db;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            _db.Update(ticket);
            await _db.SaveChangesAsync();
            return ticket;
        }
    }
}
