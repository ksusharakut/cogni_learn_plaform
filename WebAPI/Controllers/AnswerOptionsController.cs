using Application.Use_Cases.AnswerOption.CreateAnswerOption;
using Application.Use_Cases.AnswerOption.DeleteAnswerOption;
using Application.Use_Cases.AnswerOption.DTOs;
using Application.Use_Cases.AnswerOption.GetAnswerOption;
using Application.Use_Cases.AnswerOption.GetAnswerOptionsForQuestion;
using Application.Use_Cases.AnswerOption.UpdateAnswerOption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/answer-options")]
    [ApiController]
    public class AnswerOptionsController : ControllerBase
    {
        private readonly ICreateAnswerOptionUseCase _createAnswerOptionUseCase;
        private readonly IGetAnswerOptionUseCase _getAnswerOptionUseCase;
        private readonly IUpdateAnswerOptionUseCase _updateAnswerOptionUseCase;
        private readonly IDeleteAnswerOptionUseCase _deleteAnswerOptionUseCase;
        private readonly IGetAnswerOptionsForQuestionUseCase _getAnswerOptionsForQuestionUseCase;

        public AnswerOptionsController(
            ICreateAnswerOptionUseCase createAnswerOptionUseCase,
            IGetAnswerOptionUseCase getAnswerOptionUseCase,
            IUpdateAnswerOptionUseCase updateAnswerOptionUseCase,
            IDeleteAnswerOptionUseCase deleteAnswerOptionUseCase,
            IGetAnswerOptionsForQuestionUseCase getAnswerOptionsForQuestionUseCase)
        {
            _createAnswerOptionUseCase = createAnswerOptionUseCase;
            _getAnswerOptionUseCase = getAnswerOptionUseCase;
            _updateAnswerOptionUseCase = updateAnswerOptionUseCase;
            _deleteAnswerOptionUseCase = deleteAnswerOptionUseCase;
            _getAnswerOptionsForQuestionUseCase = getAnswerOptionsForQuestionUseCase;
        }

        [HttpPost("question/{questionId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateAnswerOption(int questionId, [FromBody] CreateAnswerOptionDTO request, CancellationToken cancellationToken)
        {
            var answerOptionId = await _createAnswerOptionUseCase.ExecuteAsync(questionId, request, cancellationToken);
            return Ok(new { AnswerOptionId = answerOptionId, Message = "Вариант ответа успешно создан!" });
        }

        [HttpGet("{answerOptionId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetAnswerOption(int answerOptionId, CancellationToken cancellationToken)
        {
            var answerOption = await _getAnswerOptionUseCase.ExecuteAsync(answerOptionId, cancellationToken);
            return Ok(answerOption);
        }

        [HttpPut("{answerOptionId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateAnswerOption(int answerOptionId, [FromBody] CreateAnswerOptionDTO request, CancellationToken cancellationToken)
        {
            await _updateAnswerOptionUseCase.ExecuteAsync(answerOptionId, request, cancellationToken);
            return Ok("Вариант ответа успешно обновлён!");
        }

        [HttpDelete("{answerOptionId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> DeleteAnswerOption(int answerOptionId, CancellationToken cancellationToken)
        {
            await _deleteAnswerOptionUseCase.ExecuteAsync(answerOptionId, cancellationToken);
            return Ok("Вариант ответа успешно удалён!");
        }

        [HttpGet("question/{questionId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetAnswerOptionsForQuestion(int questionId, CancellationToken cancellationToken)
        {
            var answerOptions = await _getAnswerOptionsForQuestionUseCase.ExecuteAsync(questionId, cancellationToken);
            return Ok(answerOptions);
        }

    }
}
