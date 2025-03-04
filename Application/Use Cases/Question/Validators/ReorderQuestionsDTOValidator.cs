using Application.Use_Cases.Question.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Question.Validators
{
    public class ReorderQuestionsDTOValidator : AbstractValidator<ReorderQuestionsDTO>
    {
        public ReorderQuestionsDTOValidator()
        {
            RuleFor(x => x.Questions)
                .NotNull().WithMessage("Список вопросов не может быть пустым.")
                .NotEmpty().WithMessage("Список вопросов не может быть пустым.")
                .Must(questions => questions.Select(q => q.QuestionId).Distinct().Count() == questions.Count)
                .WithMessage("Идентификаторы вопросов не должны повторяться.");

            RuleForEach(x => x.Questions)
                .ChildRules(question =>
                {
                    question.RuleFor(q => q.QuestionId)
                        .GreaterThan(0).WithMessage("QuestionId должен быть больше 0.");

                    question.RuleFor(q => q.OrderIndex)
                        .GreaterThanOrEqualTo(0).WithMessage("OrderIndex должен быть больше или равен 0.");
                });
        }
    }
}
