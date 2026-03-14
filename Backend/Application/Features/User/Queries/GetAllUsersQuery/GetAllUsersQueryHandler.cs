using Application.Features.User.DTOs;
using Domain.AggregateModels.Users;
using MediatR;

namespace Application.Features.User.Queries.GetAllUsersQuery
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserFinder _userFinder;

        public GetAllUsersQueryHandler(IUserFinder userFinder)
        {
            _userFinder = userFinder;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userFinder.GetAllAsync();
            var result = users
                .Select(user => new UserDto(user.Id, user.Username, user.Role.ToString()))
                .ToList();

            return result;
        }
    }
}
