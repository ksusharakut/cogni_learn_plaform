namespace Domain.Entities
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int UserRating { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string ReviewDescription { get; set; }

        public Course Course { get; set; }
        public User User { get; set; }
    }
}
