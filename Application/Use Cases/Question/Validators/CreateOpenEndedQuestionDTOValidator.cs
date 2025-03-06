using Application.Use_Cases.Question.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Question.Validators
{
    public class CreateOpenEndedQuestionDTOValidator : AbstractValidator<CreateOpenEndedQuestionDTO>
    {
        public CreateOpenEndedQuestionDTOValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст вопроса обязателен.")
                .MaximumLength(500).WithMessage("Текст вопроса не должен превышать 500 символов.");

            RuleFor(x => x.CorrectAnswer)
                .NotEmpty().WithMessage("Для открытых вопросов необходимо указать правильный ответ.")
                .MaximumLength(255).WithMessage("Правильный ответ не должен превышать 255 символов.");
        }
    }
}
