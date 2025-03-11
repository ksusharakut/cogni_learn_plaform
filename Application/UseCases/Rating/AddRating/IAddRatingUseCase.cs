using Application.Use_Cases.Rating.DTOs;

namespace Application.Use_Cases.Rating.AddRating
{
    public interface IAddRatingUseCase
    {
        Task<int> ExecuteAsync(int courseId, AddRatingDTO request, CancellationToken cancellationToken);
    }
}
