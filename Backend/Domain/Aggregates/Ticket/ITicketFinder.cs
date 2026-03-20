namespace Domain.AggregateModels.Tickets
{
    public interface ITicketFinder
    {
        Task<Ticket?> GetTicketByIdAsync(Guid id);

        Task<IEnumerable<Ticket>> GetAllTicketsAsync();

        Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(string? status,
            string? querySearch, string? sort);
    }
}
