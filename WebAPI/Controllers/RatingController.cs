using Application.Use_Cases.Rating.AddRating;
using Application.Use_Cases.Rating.DeleteRating;
using Application.Use_Cases.Rating.DTOs;
using Application.Use_Cases.Rating.GetAverageCourseRating;
using Application.Use_Cases.Rating.GetCourseRatings;
using Application.Use_Cases.Rating.GetUserRatingForCourse;
using Application.Use_Cases.Rating.UpdateRating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IAddRatingUseCase _addRatingUseCase;
        private readonly IDeleteRatingUseCase _deleteRatingUseCase;
        private readonly IUpdateRatingUseCase _updateRatingUseCase;
        private readonly IGetCourseRatingsUseCase _getCourseRatingsUseCase;
        private readonly IGetUserRatingForCourseUseCase _getUserRatingForCourseUseCase;
        private readonly IGetAverageCourseRatingUseCase _getAverageCourseRatingUseCase;

        public RatingController(IAddRatingUseCase addRatingUseCase,
            IDeleteRatingUseCase deleteRatingUseCase,
            IUpdateRatingUseCase updateRatingUseCase,
            IGetCourseRatingsUseCase getCourseRatingsUseCase,
            IGetUserRatingForCourseUseCase getUserRatingForCourseUseCase,
            IGetAverageCourseRatingUseCase getAverageCourseRatingUseCase)
        {
            _addRatingUseCase = addRatingUseCase;
            _deleteRatingUseCase = deleteRatingUseCase;
            _updateRatingUseCase = updateRatingUseCase;
            _getCourseRatingsUseCase = getCourseRatingsUseCase;
            _getUserRatingForCourseUseCase = getUserRatingForCourseUseCase;
            _getAverageCourseRatingUseCase = getAverageCourseRatingUseCase;
        }

        [HttpPost("course/{courseId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> AddRating(int courseId, [FromBody] AddRatingDTO request, CancellationToken cancellationToken)
        {
            var ratingId = await _addRatingUseCase.ExecuteAsync(courseId, request, cancellationToken);
            return Ok(new { RatingId = ratingId, Message = "Рейтинг успешно добавлен!" });
        }

        [HttpDelete("{ratingId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> DeleteRating(int ratingId, CancellationToken cancellationToken)
        {
            await _deleteRatingUseCase.ExecuteAsync(ratingId, cancellationToken);
            return Ok("Рейтинг успешно удалён!");
        }

        [HttpPut("{ratingId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] AddRatingDTO request, CancellationToken cancellationToken)
        {
            await _updateRatingUseCase.ExecuteAsync(ratingId, request, cancellationToken);
            return Ok("Рейтинг успешно обновлён!");
        }

        [HttpGet("course/{courseId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetCourseRatings(int courseId, CancellationToken cancellationToken)
        {
            var ratings = await _getCourseRatingsUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok(ratings);
        }

        [HttpGet("my/course/{courseId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetUserRatingForCourse(int courseId, CancellationToken cancellationToken)
        {
            var result = await _getUserRatingForCourseUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("course/{courseId}/average")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetAverageCourseRating(int courseId, CancellationToken cancellationToken)
        {
            var averageRating = await _getAverageCourseRatingUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok(averageRating);
        }
    }
}
