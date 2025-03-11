using Application.Exceptions;
using Application.Use_Cases.Rating.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Rating.GetAverageCourseRating
{
    public class GetAverageCourseRatingUseCase : IGetAverageCourseRatingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAverageCourseRatingUseCase(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AverageRatingDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken)
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

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            var ratings = await _unitOfWork.RatingRepository.GetByCourseIdAsync(courseId, cancellationToken);
            var ratingList = ratings.ToList();

            if (!ratingList.Any())
            {
                return new AverageRatingDTO
                {
                    CourseId = courseId,
                    AverageRating = 0.0,
                    RatingCount = 0
                };
            }

            var averageRating = ratingList.Average(r => r.UserRating);
            var ratingCount = ratingList.Count;

            return new AverageRatingDTO
            {
                CourseId = courseId,
                AverageRating = Math.Round(averageRating, 2), 
                RatingCount = ratingCount
            };
        }
    }
}
