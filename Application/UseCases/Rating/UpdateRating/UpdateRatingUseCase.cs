using Application.Exceptions;
using Application.Use_Cases.Rating.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Rating.UpdateRating
{
    public class UpdateRatingUseCase : IUpdateRatingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddRatingDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateRatingUseCase(
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

        public async Task ExecuteAsync(int ratingId, AddRatingDTO request, CancellationToken cancellationToken)
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

            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(ratingId, cancellationToken);
            if (rating == null)
            {
                throw new NotFoundException($"Рейтинг с идентификатором {ratingId} не найден.");
            }

            if (rating.UserId != userId)
            {
                throw new AuthorizationException("Вы можете обновлять только свои собственные рейтинги.");
            }

            _mapper.Map(request, rating);
            rating.UpdatedAt = DateTimeOffset.UtcNow;

            _unitOfWork.RatingRepository.Update(rating);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
