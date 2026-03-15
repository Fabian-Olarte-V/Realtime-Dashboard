namespace Application.Features.Ticket.DTOs
{
    public sealed record TicketDto(
        Guid Id,
        string Title,
        string? Description,
        string Status,
        Guid? AssigneeId,
        int Version,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt,
        DateTimeOffset? DeadlineAt
    );

    public sealed record TicketMutationResponseDto(TicketDto Items, DateTimeOffset ServerTime);
}
