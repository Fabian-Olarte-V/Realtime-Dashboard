namespace Application.Features.Auth.DTOs
{
    public sealed record AuthUserDto(Guid Id, string Username, string Role);
    public sealed record LoginRequestDto(string Username, string Password);
    public sealed record AuthResponseDto(string Token, AuthUserDto User);
}
