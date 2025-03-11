using Application.Use_Cases.Rating.DTOs;

namespace Application.Use_Cases.Rating.GetAverageCourseRating
{
    public interface IGetAverageCourseRatingUseCase
    {
        Task<AverageRatingDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
