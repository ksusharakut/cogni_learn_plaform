namespace Application.Use_Cases.Rating.DeleteRating
{
    public interface IDeleteRatingUseCase
    {
        Task ExecuteAsync(int ratingId, CancellationToken cancellationToken);
    }
}
