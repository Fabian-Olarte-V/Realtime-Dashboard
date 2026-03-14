using Application.Features.User.DTOs;
using MediatR;

namespace Application.Features.User.Queries.GetAllUsersQuery
{
    public class GetAllUsersQuery: IRequest<IEnumerable<UserDto>>
    {
        public GetAllUsersQuery() { }
    }
}
