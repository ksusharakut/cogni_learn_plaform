using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
    {
        public void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            builder.HasKey(uc => uc.UserCourseId);

            builder.Property(uc => uc.UserCourseId)
                .ValueGeneratedOnAdd();

            builder.Property(uc => uc.CreatedAt)
                .IsRequired();

            builder.Property(uc => uc.CompletionStatus)
                .IsRequired();

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
