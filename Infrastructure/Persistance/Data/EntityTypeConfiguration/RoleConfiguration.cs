using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityTypeConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.RoleName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    j => j.HasKey("UserId", "RoleId")
                );
        }
    }
}
