using Application.Exceptions;
using Application.Use_Cases.Rating.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Rating.GetUserRatingForCourse
{
    public class GetUserRatingForCourseUseCase : IGetUserRatingForCourseUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserRatingForCourseUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserRatingResultDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken)
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

            var rating = await _unitOfWork.RatingRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken);
            if (rating == null)
            {
                return UserRatingResultDTO.NoRating("Вы ещё не оставили рейтинг для этого курса.");
            }

            return UserRatingResultDTO.Success(_mapper.Map<RatingDTO>(rating));
        }
    }
}
