namespace Application.Use_Cases.Rating.DTOs
{
    public class AverageRatingDTO
    {
        public int CourseId { get; set; }
        public double AverageRating { get; set; } 
        public int RatingCount { get; set; }
    }
}
