using Application.Use_Cases.Chapter.CreateChapter;
using Application.Use_Cases.Chapter.DeleteChapter;
using Application.Use_Cases.Chapter.DTOs;
using Application.Use_Cases.Chapter.ReorderChapters;
using Application.Use_Cases.Chapter.UpdateChapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/chapters")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly ICreateChapterUseCase _createChapterUseCase;
        private readonly IDeleteChapterUseCase _deleteChapterUseCase;
        private readonly IUpdateChapterUseCase _updateChapterUseCase;
        private readonly IReorderChaptersUseCase _reorderChaptersUseCase;

        public ChaptersController(ICreateChapterUseCase createChapterUseCase, 
            IDeleteChapterUseCase deleteChapterUseCase,
            IUpdateChapterUseCase updateChapterUseCase,
            IReorderChaptersUseCase reorderChaptersUseCase)
        {
            _createChapterUseCase = createChapterUseCase;
            _deleteChapterUseCase = deleteChapterUseCase;
            _updateChapterUseCase = updateChapterUseCase;
            _reorderChaptersUseCase = reorderChaptersUseCase;
        }

        [HttpPost("course/{courseId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateChapter(int courseId, [FromBody] CreateChapterDTO request, CancellationToken cancellationToken)
        {
            var chapterId = await _createChapterUseCase.ExecuteAsync(courseId, request, cancellationToken);
            return Ok(new { ChapterId = chapterId, Message = "Глава успешно создана!" });
        }

        [HttpDelete("{chapterId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> DeleteChapter(int chapterId, CancellationToken cancellationToken)
        {
            await _deleteChapterUseCase.ExecuteAsync(chapterId, cancellationToken);
            return Ok("Глава успешно удалена!");
        }

        [HttpPut("{chapterId}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateChapter(int chapterId, [FromBody] CreateChapterDTO request, CancellationToken cancellationToken)
        {
            await _updateChapterUseCase.ExecuteAsync(chapterId, request, cancellationToken);
            return Ok("Название главы успешно обновлено!");
        }

        [HttpPut("course/{courseId}/reorder")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> ReorderChapters(int courseId, [FromBody] ReorderChaptersDTO request, CancellationToken cancellationToken)
        {
            await _reorderChaptersUseCase.ExecuteAsync(courseId, request, cancellationToken);
            return Ok("Главы успешно переупорядочены!");
        }
    }
}
