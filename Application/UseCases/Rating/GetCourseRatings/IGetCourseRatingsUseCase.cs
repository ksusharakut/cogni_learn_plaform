using Application.Use_Cases.Rating.DTOs;

namespace Application.Use_Cases.Rating.GetCourseRatings
{
    public interface IGetCourseRatingsUseCase
    {
        Task<IEnumerable<RatingDTO>> ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
