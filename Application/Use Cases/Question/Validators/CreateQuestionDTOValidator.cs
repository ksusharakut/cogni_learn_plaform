using Application.Use_Cases.Question.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Question.Validators
{
    public class CreateQuestionDTOValidator : AbstractValidator<CreateQuestionDTO>
    {
        public CreateQuestionDTOValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст вопроса обязателен.")
                .MaximumLength(500).WithMessage("Текст вопроса не должен превышать 500 символов.");

            RuleFor(x => x.QuestionType)
                .IsInEnum().WithMessage("Неверный тип вопроса.");
        }
    }
}
