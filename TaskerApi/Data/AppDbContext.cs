using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TaskerApi.Models;

namespace TaskerApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ATask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<ATask>().HasOne<User>().WithMany().HasForeignKey(t=>t.OwnerId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().Property(u => u.RefreshToken).IsRequired(false);
        }
    }
}
