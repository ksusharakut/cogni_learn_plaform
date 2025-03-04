using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;


namespace Application.Use_Cases.User.DeleteUser
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteUserUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Unable to identify the current user.");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task ExecuteAsync(int userId, CancellationToken cancellationToken)
        {
            await DeleteUserInternal(userId, cancellationToken);
        }

        private async Task DeleteUserInternal(int userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
