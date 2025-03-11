using Application.Use_Cases.Question.DTOs;

namespace Application.Use_Cases.Question.GetQuestionsForLesson
{
    public interface IGetQuestionsForLessonUseCase
    {
        Task<IEnumerable<QuestionDTO>> ExecuteAsync(int lessonId, CancellationToken cancellationToken);
    }
}
