using GestionImmo.Models.Entities;
using GestionImmo.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace GestionImmo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Favorite> Favorites { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("3ab35ede-7489-49f6-af05-f7043cd74093"),
                    FullName = "admin S",
                    email = "se@admin.com",
                    password = "password123",
                    address = "Tunis",
                    Role = Role.ADMIN,
                    phone = "20202020"
                },
                new User
                {
                    Id = Guid.Parse("95bd3141-36a9-4dd6-adf1-291875bb8c83"),
                    FullName = "agent S",
                    email = "s@eagent.com",
                    password = "password456",
                    address = "Ariana",
                    Role = Role.AGENT,
                    phone = "30303030"
                }
            );

            modelBuilder.Entity<Property>()
                .HasOne(p => p.User)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Property)
                .WithMany(p => p.Visits)
                .HasForeignKey(v => v.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.User)
                .WithMany(u => u.Visits)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Photo>()
                .HasOne(ph => ph.Property)
                .WithMany(p => p.Photos)
                .HasForeignKey(ph => ph.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Property)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.CreatedBy)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
