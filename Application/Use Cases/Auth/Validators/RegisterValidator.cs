using FluentValidation;

namespace Application.Use_Cases.Auth.DTOs.Validators
{
    public class RegisterValidator : AbstractValidator<UserRegisterDTO>
    {
        public RegisterValidator()
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

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Пароль обязателен")
                    .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов")
                    .Matches("[A-ZА-Я]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву (A-Z, А-Я)")
                    .Matches("[a-zа-я]").WithMessage("Пароль должен содержать хотя бы одну строчную букву (a-z, а-я)")
                    .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру");
        }
    }
}
