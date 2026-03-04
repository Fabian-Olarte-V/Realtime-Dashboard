namespace Application.DTOs.Ticket
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

    public sealed record AssingTicketRequestDto(int ExpectedVersion);
    public sealed record CompleteTicketRequestDto(TicketDto Item, DateTimeOffset ServerTime);
    public sealed record TicketMutationResponseDto(TicketDto Items, DateTimeOffset ServerTime);
}
