namespace Application.Use_Cases.Rating.DTOs
{
    public class RatingDTO
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public int UserRating { get; set; }
        public string ReviewDescription { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
