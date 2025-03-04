using Application.Use_Cases.User.DTOs;
using FluentValidation;

namespace Application.Use_Cases.User.Validators
{
    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Старый пароль обязателен для ввода.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Новый пароль обязателен для ввода.")
                .MinimumLength(6).WithMessage("Новый пароль должен содержать минимум 6 символов.")
                .Matches(@"^(?=.*[a-zA-Z])(?=.*\d).*$").WithMessage("Новый пароль должен содержать хотя бы одну букву и одну цифру.")
                .NotEqual(x => x.OldPassword).WithMessage("Новый пароль не должен совпадать со старым.");
        }
    }
}
