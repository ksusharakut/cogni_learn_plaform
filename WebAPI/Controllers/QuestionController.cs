using Application.Use_Cases.Question.CreateQuestion;
using Application.Use_Cases.Question.DeleteQuestion;
using Application.Use_Cases.Question.DTOs;
using Application.Use_Cases.Question.GetQuestion;
using Application.Use_Cases.Question.GetQuestionsForLesson;
using Application.Use_Cases.Question.ReorderQuestions;
using Application.Use_Cases.Question.UpdateQuestion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ICreateMultipleChoiceQuestionUseCase _createMultipleChoiceQuestionUseCase;
        private readonly ICreateOpenEndedQuestionUseCase _createOpenEndedQuestionUseCase;
        private readonly IGetQuestionUseCase _getQuestionUseCase;
        private readonly IUpdateMultipleChoiceQuestionUseCase _updateMultipleChoiceQuestionUseCase;
        private readonly IUpdateOpenEndedQuestionUseCase _updateOpenEndedQuestionUseCase;
        private readonly IDeleteQuestionUseCase _deleteQuestionUseCase;
        private readonly IReorderQuestionsUseCase _reorderQuestionsUseCase;
        private readonly IGetQuestionsForLessonUseCase _getQuestionsForLessonUseCase;

        public QuestionController(
            ICreateMultipleChoiceQuestionUseCase createMultipleChoiceQuestionUseCase,
            ICreateOpenEndedQuestionUseCase createOpenEndedQuestionUseCase,
            IGetQuestionUseCase getQuestionUseCase,
            IUpdateMultipleChoiceQuestionUseCase updateMultipleChoiceQuestionUseCase,
            IUpdateOpenEndedQuestionUseCase updateOpenEndedQuestionUseCase,
            IDeleteQuestionUseCase deleteQuestionUseCase,
            IReorderQuestionsUseCase reorderQuestionsUseCase,
            IGetQuestionsForLessonUseCase getQuestionsForLessonUseCase)
        {
            _createMultipleChoiceQuestionUseCase = createMultipleChoiceQuestionUseCase;
            _createOpenEndedQuestionUseCase = createOpenEndedQuestionUseCase;
            _getQuestionUseCase = getQuestionUseCase;
            _updateMultipleChoiceQuestionUseCase = updateMultipleChoiceQuestionUseCase;
            _updateOpenEndedQuestionUseCase = updateOpenEndedQuestionUseCase;
            _deleteQuestionUseCase = deleteQuestionUseCase;
            _reorderQuestionsUseCase = reorderQuestionsUseCase;
            _getQuestionsForLessonUseCase = getQuestionsForLessonUseCase;
        }

        [HttpPost("lesson/{lessonId}/multiple-choice")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateMultipleChoiceQuestion(int lessonId, [FromBody] CreateMultipleChoiceQuestionDTO request, CancellationToken cancellationToken)
        {
            var questionId = await _createMultipleChoiceQuestionUseCase.ExecuteAsync(lessonId, request, cancellationToken);
            return Ok("Вопрос с множественным выбором успешно создан!" );
        }

        [HttpPost("lesson/{lessonId}/open-ended")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateOpenEndedQuestion(int lessonId, [FromBody] CreateOpenEndedQuestionDTO request, CancellationToken cancellationToken)
        {
            var questionId = await _createOpenEndedQuestionUseCase.ExecuteAsync(lessonId, request, cancellationToken);
            return Ok("Открытый вопрос успешно создан!");
        }

        [HttpPut("{questionId}/multiple-choice")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateMultipleChoiceQuestion(int questionId, [FromBody] CreateMultipleChoiceQuestionDTO request, CancellationToken cancellationToken)
        {
            await _updateMultipleChoiceQuestionUseCase.ExecuteAsync(questionId, request, cancellationToken);
            return Ok(new { Message = "Вопрос с множественным выбором успешно обновлён!" });
        }

        [HttpPut("{questionId}/open-ended")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateOpenEndedQuestion(int questionId, [FromBody] CreateOpenEndedQuestionDTO request, CancellationToken cancellationToken)
        {
            await _updateOpenEndedQuestionUseCase.ExecuteAsync(questionId, request, cancellationToken);
            return Ok(new { Message = "Открытый вопрос успешно обновлён!" });
        }

        [HttpGet("{questionId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetQuestion(int questionId, CancellationToken cancellationToken)
        {
            var question = await _getQuestionUseCase.ExecuteAsync(questionId, cancellationToken);
            return Ok(question);
        }

        [HttpDelete("{questionId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> DeleteQuestion(int questionId, CancellationToken cancellationToken)
        {
            await _deleteQuestionUseCase.ExecuteAsync(questionId, cancellationToken);
            return Ok("Вопрос успешно удалён!");
        }

        [HttpPut("lesson/{lessonId}/reorder")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> ReorderQuestions(int lessonId, [FromBody] ReorderQuestionsDTO request, CancellationToken cancellationToken)
        {
            await _reorderQuestionsUseCase.ExecuteAsync(lessonId, request, cancellationToken);
            return Ok("Вопросы успешно переупорядочены!");
        }

        [HttpGet("lesson/{lessonId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetQuestionsForLesson(int lessonId, CancellationToken cancellationToken)
        {
            var questions = await _getQuestionsForLessonUseCase.ExecuteAsync(lessonId, cancellationToken);
            return Ok(questions);
        }
    }
}
