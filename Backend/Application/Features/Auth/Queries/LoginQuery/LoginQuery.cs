using Application.Features.Auth.DTOs;
using MediatR;

namespace Application.Features.Auth.Queries.LoginQuery
{
    public class LoginQuery: IRequest<LoginResponseDto>
    {
        public string Username { get; init; }
        public string Password { get; init; }

        public LoginQuery() { }
    }
}
