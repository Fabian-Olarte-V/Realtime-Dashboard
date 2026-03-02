namespace Application.DTOs.AuthDto
{
    public sealed record LoginRequestDto(string Username, string Password);
    public sealed record AuthUserDto(Guid Id, string Username, string Role);
    public sealed record LoginResponseDto(string Token, AuthUserDto User, DateTimeOffset ServerTime);
}
