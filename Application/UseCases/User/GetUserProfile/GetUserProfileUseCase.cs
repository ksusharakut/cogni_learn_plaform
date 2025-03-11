using Application.Exceptions;
using Application.Use_Cases.User.UpdateUserProfile;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.User.GetUserProfile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserProfileUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserProfileDTO> ExecuteAsync(CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Unable to identify the current user.");
            }

            return await GetUserProfileInternal(userId, cancellationToken);
        }

        public async Task<UserProfileDTO> ExecuteAsync(int userId, CancellationToken cancellationToken)
        {
            return await GetUserProfileInternal(userId, cancellationToken);
        }

        private async Task<UserProfileDTO> GetUserProfileInternal(int userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            return _mapper.Map<UserProfileDTO>(user);
        }
    }
}
