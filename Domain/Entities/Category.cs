namespace Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int CourseId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
