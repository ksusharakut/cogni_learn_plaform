using Application.Use_Cases.User.DTOs;
using FluentValidation;

namespace Application.Use_Cases.User.Validators
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileDTO>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Фамилия обязательна")
                    .Matches(@"^[a-zA-Zа-яА-Я]+$").WithMessage("Фамилия должна содержать только буквы");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Имя обязательно")
                .Matches(@"^[a-zA-Zа-яА-Я]+$").WithMessage("Имя должно содержать только буквы");

            RuleFor(x => x.DateBirth)
                .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-14)))
                .WithMessage("Вам должно быть не менее 14 лет");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный email");
        }
    }
}
