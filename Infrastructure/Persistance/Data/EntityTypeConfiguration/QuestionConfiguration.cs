using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.QuestionId);

            builder.Property(q => q.QuestionId)
                .ValueGeneratedOnAdd();

            builder.Property(q => q.Text)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(q => q.QuestionType)
                .IsRequired();

            builder.Property(q => q.OrderIndex)
                .IsRequired();

            builder.HasOne(q => q.Lesson)  
                .WithMany(l => l.Questions)
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.AnswerOptions)  
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
