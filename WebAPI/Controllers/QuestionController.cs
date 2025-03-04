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
        private readonly ICreateQuestionUseCase _createQuestionUseCase;
        private readonly IGetQuestionUseCase _getQuestionUseCase;
        private readonly IUpdateQuestionUseCase _updateQuestionUseCase;
        private readonly IDeleteQuestionUseCase _deleteQuestionUseCase;
        private readonly IReorderQuestionsUseCase _reorderQuestionsUseCase;
        private readonly IGetQuestionsForLessonUseCase _getQuestionsForLessonUseCase;

        public QuestionController(
            ICreateQuestionUseCase createQuestionUseCase,
            IGetQuestionUseCase getQuestionUseCase,
            IUpdateQuestionUseCase updateQuestionUseCase,
            IDeleteQuestionUseCase deleteQuestionUseCase,
            IReorderQuestionsUseCase reorderQuestionsUseCase,
            IGetQuestionsForLessonUseCase getQuestionsForLessonUseCase)
        {
            _createQuestionUseCase = createQuestionUseCase;
            _getQuestionUseCase = getQuestionUseCase;
            _updateQuestionUseCase = updateQuestionUseCase;
            _deleteQuestionUseCase = deleteQuestionUseCase;
            _reorderQuestionsUseCase = reorderQuestionsUseCase;
            _getQuestionsForLessonUseCase = getQuestionsForLessonUseCase;
        }

        [HttpPost("lesson/{lessonId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateQuestion(int lessonId, [FromBody] CreateQuestionDTO request, CancellationToken cancellationToken)
        {
            var questionId = await _createQuestionUseCase.ExecuteAsync(lessonId, request, cancellationToken);
            return Ok(new { QuestionId = questionId, Message = "Вопрос успешно создан!" });
        }

        [HttpGet("{questionId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetQuestion(int questionId, CancellationToken cancellationToken)
        {
            var question = await _getQuestionUseCase.ExecuteAsync(questionId, cancellationToken);
            return Ok(question);
        }

        [HttpPut("{questionId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] CreateQuestionDTO request, CancellationToken cancellationToken)
        {
            await _updateQuestionUseCase.ExecuteAsync(questionId, request, cancellationToken);
            return Ok("Вопрос успешно обновлён!");
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
