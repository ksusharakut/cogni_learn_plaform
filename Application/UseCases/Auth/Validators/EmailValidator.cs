using Application.Use_Cases.Auth.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Auth.Validators
{
    public class EmailValidator : AbstractValidator<EmailDTO>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email обязателен")
                    .EmailAddress().WithMessage("Некорректный email");
        }
    }
}
