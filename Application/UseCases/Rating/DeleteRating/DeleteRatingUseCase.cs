using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Rating.DeleteRating
{
    public class DeleteRatingUseCase : IDeleteRatingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteRatingUseCase(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(int ratingId, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");
            }

            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(ratingId, cancellationToken);
            if (rating == null)
            {
                throw new NotFoundException($"Рейтинг с идентификатором {ratingId} не найден.");
            }

            if (rating.UserId != userId)
            {
                throw new AuthorizationException("Вы можете удалять только свои собственные рейтинги.");
            }

            _unitOfWork.RatingRepository.Delete(rating);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
