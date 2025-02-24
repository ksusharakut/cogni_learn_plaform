using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(c => c.ChapterId);

            builder.Property(c => c.ChapterId)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.OrderIndex)
                .IsRequired();

            builder.Property(c => c.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.HasOne(c => c.Course)
                .WithMany(c => c.Chapters)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Lessons)
                .WithOne(l => l.Chapter)
                .HasForeignKey(l => l.ChapterId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
