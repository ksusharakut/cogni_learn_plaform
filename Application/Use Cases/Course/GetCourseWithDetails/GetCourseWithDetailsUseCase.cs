using Application.Exceptions;
using Application.Use_Cases.Course.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Course.GetCourseWithDetails
{
    public class GetCourseWithDetailsUseCase : IGetCourseWithDetailsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCourseWithDetailsUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CourseDetailsDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken)
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

            var course = await _unitOfWork.CourseRepository.GetByIdWithDetailsAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            var isEnrolled = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken) != null;
            if (!course.IsPublished && course.UserId != userId && !isEnrolled)
            {
                throw new AuthorizationException("У вас нет доступа к этому курсу.");
            }

            var ratings = await _unitOfWork.RatingRepository.GetByCourseIdAsync(courseId, cancellationToken);
            var ratingList = ratings.ToList();
            var averageRating = ratingList.Any() ? ratingList.Average(r => r.UserRating) : 0.0;
            var ratingCount = ratingList.Count;

            var courseDetails = _mapper.Map<CourseDetailsDTO>(course);
            courseDetails.AverageRating = Math.Round(averageRating, 2);
            courseDetails.RatingCount = ratingCount;

            return courseDetails;
        }
    }
}
