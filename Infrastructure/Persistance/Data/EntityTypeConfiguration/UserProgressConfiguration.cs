using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
    {
        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.ToTable("UserProgress");

            builder.HasKey(up => up.UserProgressId);

            builder.Property(up => up.UserId)
                .IsRequired();

            builder.Property(up => up.CourseId)
                .IsRequired();

            builder.Property(up => up.ChapterId)
                .IsRequired(false); 

            builder.Property(up => up.LessonId)
                .IsRequired(false); 

            builder.Property(up => up.QuestionId)
                .IsRequired(false); 

            builder.Property(up => up.AnswerOptionId)
                .IsRequired(false); 

            builder.Property(up => up.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(up => up.IsCorrect)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(up => up.CompletedAt)
                .IsRequired();

            builder.HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(up => up.Course)
                .WithMany()
                .HasForeignKey(up => up.CourseId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(up => up.Chapter)
                .WithMany()
                .HasForeignKey(up => up.ChapterId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); 

            builder.HasOne(up => up.Lesson)
                .WithMany()
                .HasForeignKey(up => up.LessonId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); 

            builder.HasOne(up => up.Question)
                .WithMany()
                .HasForeignKey(up => up.QuestionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(up => up.AnswerOption)
                .WithMany()
                .HasForeignKey(up => up.AnswerOptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasIndex(up => new { up.UserId, up.CourseId, up.ChapterId, up.LessonId, up.QuestionId })
                .IsUnique(false); 
        }
    }
}
