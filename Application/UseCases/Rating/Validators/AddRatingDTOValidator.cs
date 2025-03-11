using Application.Use_Cases.Rating.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Rating.Validators
{
    public class AddRatingDTOValidator : AbstractValidator<AddRatingDTO>
    {
        public AddRatingDTOValidator()
        {
            RuleFor(x => x.UserRating)
                .InclusiveBetween(1, 5).WithMessage("Оценка должна быть от 1 до 5.");

            RuleFor(x => x.ReviewDescription)
                .MaximumLength(500).WithMessage("Описание отзыва не должно превышать 500 символов.")
                .When(x => x.ReviewDescription != null);
        }
    }
}
