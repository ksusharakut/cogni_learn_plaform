using Application.Use_Cases.Rating.DTOs;

namespace Application.Use_Cases.Rating.UpdateRating
{
    public interface IUpdateRatingUseCase
    {
        Task ExecuteAsync(int ratingId, AddRatingDTO request, CancellationToken cancellationToken);
    }
}
