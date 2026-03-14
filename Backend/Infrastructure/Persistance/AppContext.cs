using Domain.AggregateModels.Tickets;
using Domain.AggregateModels.Users;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistance
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(x => x.Name).IsRequired().HasMaxLength(120);
                entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(x => x.PasswordHash).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Role).IsRequired();
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");
                entity.HasKey(e => e.Id);

                entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Description).IsRequired().HasMaxLength(1000);
                entity.Property(x => x.Status).IsRequired();
                entity.Property(x => x.Priority).IsRequired();
                entity.Property(x => x.DeadlineAt).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
                entity.Property(x => x.UpdatedAt).IsRequired();
                entity.Property(x => x.Version).IsRequired();
                entity.Property(x => x.FailReason).HasMaxLength(500);
                
                entity.HasOne(e => e.Assignee)
                    .WithMany()
                    .HasForeignKey(e => e.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
