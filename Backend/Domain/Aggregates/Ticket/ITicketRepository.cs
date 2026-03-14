namespace Domain.AggregateModels.Tickets
{
    public interface ITicketRepository
    {
        Task<Ticket> UpdateAsync(Ticket ticket);
    }
}
