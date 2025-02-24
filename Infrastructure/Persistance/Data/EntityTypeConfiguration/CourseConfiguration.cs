using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CourseId);

            builder.Property(c => c.CourseId)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(c => c.IsPublished)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.UserCourses)
                .WithOne(uc => uc.Course)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Ratings)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Categories)
                .WithMany(cat => cat.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                    j => j.HasKey("CourseId", "CategoryId")
                );

            builder.HasMany(c => c.Chapters)
                .WithOne(ch => ch.Course)
                .HasForeignKey(ch => ch.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
