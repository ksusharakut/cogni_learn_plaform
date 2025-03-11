namespace Application.Use_Cases.Rating.DTOs
{
    public class UserRatingResultDTO
    {
        public RatingDTO Rating { get; set; }
        public string Message { get; set; }

        public static UserRatingResultDTO Success(RatingDTO rating) => new UserRatingResultDTO { Rating = rating };
        public static UserRatingResultDTO NoRating(string message) => new UserRatingResultDTO { Message = message };
    }
}
