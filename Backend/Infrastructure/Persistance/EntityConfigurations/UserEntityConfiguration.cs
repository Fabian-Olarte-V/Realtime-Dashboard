using Domain.AggregateModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(e => e.Id);

            builder.Property(x => x.Username).IsRequired().HasMaxLength(120);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Role).IsRequired();
        }
    }
}
