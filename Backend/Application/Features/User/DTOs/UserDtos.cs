namespace Application.Features.User.DTOs
{
    public sealed record UserDto(Guid Id, string Username, string Role);
}
