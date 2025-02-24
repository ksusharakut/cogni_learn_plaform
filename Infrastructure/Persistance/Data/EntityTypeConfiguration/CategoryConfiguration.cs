using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.CategoryId)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CategoryName)
                .HasMaxLength(200) 
                .IsRequired();

            builder.HasMany(c => c.Courses)
                .WithMany(c => c.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseCategory",  
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasKey("CourseId", "CategoryId")
                    );
        }
    }
}
