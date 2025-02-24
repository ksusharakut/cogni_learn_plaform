using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.LessonId);

            builder.Property(l => l.LessonId)
                .ValueGeneratedOnAdd();

            builder.Property(l => l.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.ContentPath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(l => l.OrderIndex)
                .IsRequired();

            builder.Property(l => l.UpdatedAt)
                .IsRequired();

            builder.HasOne(l => l.Chapter)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Questions)
                .WithOne(q => q.Lesson)
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
