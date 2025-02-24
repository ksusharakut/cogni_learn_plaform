using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class AnswerOptionConfiguration : IEntityTypeConfiguration<AnswerOption>
    {
        public void Configure(EntityTypeBuilder<AnswerOption> builder)
        {
            builder.HasKey(a => a.AnswerOptionId);

            builder.Property(a => a.AnswerOptionId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Text)
                .HasMaxLength(500)  
                .IsRequired();

            builder.Property(a => a.IsCorrect)
                .IsRequired();

            builder.HasOne(a => a.Question)  
                .WithMany(q => q.AnswerOptions)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
