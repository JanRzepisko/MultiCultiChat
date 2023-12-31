using MediatR;
using MultiCultiChat.App.Application.DataAccess;
using MultiCultiChat.App.Application.Jwt;
using MultiCultiChat.App.Application.Services;
using Shared.BaseModels.Exceptions;
using Shared.Service.Interfaces;

namespace MultiCultiChat.App.Application.Actions.Auth;

public static class RefreshToken
{
    public sealed record Query : IRequest<GeneratedToken>;

    public class Handler : IRequestHandler<Query, GeneratedToken>
    {
        private readonly IJwtAuth _jwtAuth;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProvider _userProvider;

        public Handler(IUnitOfWork unitOfWork, IJwtAuth jwtAuth, IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _jwtAuth = jwtAuth;
            _userProvider = userProvider;
        }

        public async Task<GeneratedToken> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(_userProvider.Id, cancellationToken);
            if (user is null) throw new EntityNotFoundException("User not found");
            return await _jwtAuth.GenerateJwt(user.Id, user.Email, user.Username);
        }
    }
}