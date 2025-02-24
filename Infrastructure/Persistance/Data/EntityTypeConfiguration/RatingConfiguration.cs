using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.RatingId);

            builder.Property(r => r.RatingId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.UserRating)
                .IsRequired();

            builder.Property(r => r.ReviewDescription)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(r => r.UpdatedAt)
                .IsRequired();

            builder.HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.HasOne(r => r.Course)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
