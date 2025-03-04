using Application.Use_Cases.Lesson;
using Application.Use_Cases.Lesson.CreateLesson;
using Application.Use_Cases.Lesson.DeleteLesson;
using Application.Use_Cases.Lesson.DTOs;
using Application.Use_Cases.Lesson.GetLesson;
using Application.Use_Cases.Lesson.GetLessonsForChapter;
using Application.Use_Cases.Lesson.ReorderLessons;
using Application.Use_Cases.Lesson.UpdateLesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/lessons")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ICreateLessonUseCase _createLessonUseCase;
        private readonly IGetLessonUseCase _getLessonUseCase;
        private readonly IUpdateLessonUseCase _updateLessonUseCase;
        private readonly IDeleteLessonUseCase _deleteLessonUseCase;
        private readonly IReorderLessonsUseCase _reorderLessonsUseCase;
        private readonly IGetLessonsForChapterUseCase _getLessonsForChapterUseCase;

        public LessonsController(
            ICreateLessonUseCase createLessonUseCase,
            IGetLessonUseCase getLessonUseCase,
            IUpdateLessonUseCase updateLessonUseCase,
            IDeleteLessonUseCase deleteLessonUseCase,
            IReorderLessonsUseCase reorderLessonsUseCase,
            IGetLessonsForChapterUseCase getLessonsForChapterUseCase)
        {
            _createLessonUseCase = createLessonUseCase;
            _getLessonUseCase = getLessonUseCase;
            _updateLessonUseCase = updateLessonUseCase;
            _deleteLessonUseCase = deleteLessonUseCase;
            _reorderLessonsUseCase = reorderLessonsUseCase;
            _getLessonsForChapterUseCase = getLessonsForChapterUseCase;
        }

        [HttpPost("chapter/{chapterId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateLesson(int chapterId, [FromBody] CreateLessonDTO request, CancellationToken cancellationToken)
        {
            var lessonId = await _createLessonUseCase.ExecuteAsync(chapterId, request, cancellationToken);
            return Ok(new { LessonId = lessonId, Message = "Урок успешно создан!" });
        }

        [HttpGet("{lessonId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetLesson(int lessonId, CancellationToken cancellationToken)
        {
            var lesson = await _getLessonUseCase.ExecuteAsync(lessonId, cancellationToken);
            return Ok(lesson);
        }

        [HttpPut("{lessonId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateLesson(int lessonId, [FromBody] CreateLessonDTO request, CancellationToken cancellationToken)
        {
            await _updateLessonUseCase.ExecuteAsync(lessonId, request, cancellationToken);
            return Ok("Урок успешно обновлён!");
        }

        [HttpDelete("{lessonId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> DeleteLesson(int lessonId, CancellationToken cancellationToken)
        {
            await _deleteLessonUseCase.ExecuteAsync(lessonId, cancellationToken);
            return Ok("Урок успешно удалён!");
        }

        [HttpPut("chapter/{chapterId}/reorder")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> ReorderLessons(int chapterId, [FromBody] ReorderLessonsDTO request, CancellationToken cancellationToken)
        {
            await _reorderLessonsUseCase.ExecuteAsync(chapterId, request, cancellationToken);
            return Ok("Уроки успешно переупорядочены!");
        }

        [HttpGet("chapter/{chapterId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetLessonsForChapter(int chapterId, CancellationToken cancellationToken)
        {
            var lessons = await _getLessonsForChapterUseCase.ExecuteAsync(chapterId, cancellationToken);
            return Ok(lessons);
        }
    }
}
