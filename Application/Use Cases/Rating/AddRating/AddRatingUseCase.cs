using Application.Exceptions;
using Application.Use_Cases.Rating.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Rating.AddRating
{
    public class AddRatingUseCase : IAddRatingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddRatingDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AddRatingUseCase(
            IUnitOfWork unitOfWork,
            IValidator<AddRatingDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(int courseId, AddRatingDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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

            var enrollment = await _unitOfWork.UserCourseRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken);
            if (enrollment == null)
            {
                throw new AuthorizationException("Вы не записаны на этот курс и не можете оставить рейтинг.");
            }

            var existingRating = await _unitOfWork.RatingRepository.GetByUserAndCourseAsync(userId, courseId, cancellationToken);
            if (existingRating != null)
            {
                throw new ConflictException("Вы уже оставили рейтинг для этого курса.");
            }

            var rating = _mapper.Map<Domain.Entities.Rating>(request);
            rating.UserId = userId;
            rating.UpdatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.RatingRepository.AddAsync(rating, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rating.RatingId;
        }
    }
}
