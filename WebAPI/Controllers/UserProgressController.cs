using Application.Use_Cases.UserProgress;
using Application.Use_Cases.UserProgress.CompleteLesson;
using Application.Use_Cases.UserProgress.DTOs;
using Application.Use_Cases.UserProgress.GetCourseProgress;
using Application.Use_Cases.UserProgress.SubmitAnswer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/user-progress")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly ICompleteLessonUseCase _completeLessonUseCase;
        private readonly IGetCourseProgressUseCase _getCourseProgressUseCase;
        private readonly IResetLessonProgressUseCase _resetLessonProgressUseCase;
        private readonly ISubmitMultipleChoiceAnswerUseCase _submitMultipleChoiceAnswerUseCase;
        private readonly ISubmitOpenEndedAnswerUseCase _submitOpenEndedAnswerUseCase;

        public UserProgressController(
            ICompleteLessonUseCase completeLessonUseCase,
            ISubmitMultipleChoiceAnswerUseCase submitMultipleChoiceAnswerUseCase,
            ISubmitOpenEndedAnswerUseCase submitOpenEndedAnswerUseCase,
            IGetCourseProgressUseCase getCourseProgressUseCase,
            IResetLessonProgressUseCase resetLessonProgressUseCase)
        {
            _completeLessonUseCase = completeLessonUseCase;
            _submitMultipleChoiceAnswerUseCase = submitMultipleChoiceAnswerUseCase;
            _submitOpenEndedAnswerUseCase = submitOpenEndedAnswerUseCase;
            _getCourseProgressUseCase = getCourseProgressUseCase;
            _resetLessonProgressUseCase = resetLessonProgressUseCase;
        }

        [HttpPost("question/{questionId}/submit/multiple-choice")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> SubmitMultipleChoiceAnswer(int questionId, [FromBody] SubmitMultipleChoiceAnswerDTO request, CancellationToken cancellationToken)
        {
            var result = await _submitMultipleChoiceAnswerUseCase.ExecuteAsync(questionId, request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("question/{questionId}/submit/open-ended")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> SubmitOpenEndedAnswer(int questionId, [FromBody] SubmitOpenEndedAnswerDTO request, CancellationToken cancellationToken)
        {
            var result = await _submitOpenEndedAnswerUseCase.ExecuteAsync(questionId, request, cancellationToken);
            return Ok(result);
        }

            [HttpPost("lesson/{lessonId}/complete")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> CompleteLesson(int lessonId, CancellationToken cancellationToken)
        {
            await _completeLessonUseCase.ExecuteAsync(lessonId, cancellationToken);
            return Ok("Урок успешно завершён!");
        }

        [HttpGet("course/{courseId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetCourseProgress(int courseId, CancellationToken cancellationToken)
        {
            var progress = await _getCourseProgressUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok(progress);
        }

        [HttpPost("lesson/{lessonId}/reset")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> ResetLessonProgress(int lessonId, CancellationToken cancellationToken)
        {
            await _resetLessonProgressUseCase.ExecuteAsync(lessonId, cancellationToken);
            return Ok("Прогресс урока успешно сброшен!");
        }
    }
}
