using Application.Use_Cases.Auth.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Auth.Validators
{
    public class LogInValidator : AbstractValidator<LogInUserDTO>
    {
        public LogInValidator()
        {
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
