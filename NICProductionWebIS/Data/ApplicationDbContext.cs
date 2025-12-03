using Microsoft.EntityFrameworkCore;
using NICProductionWebIS.Models;

namespace NICProductionWebIS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<NicModel> NicTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NicModel>()
            .ToTable("NicTable", "public");

            modelBuilder.Entity<NicModel>()
            .Property(x => x.Gender)
            .HasConversion(
                v => v == Gender.Male ? "M" : "F",                   
                v => v == "M" ? Gender.Male : Gender.Female
            );

            modelBuilder.Entity<NicModel>()
            .Property(x => x.BornDate)
            .HasColumnType("date");

            modelBuilder.Entity<NicModel>()
            .Property(x => x.IssueDate)
            .HasColumnType("date");

            modelBuilder.Entity<NicModel>()
            .Property(x => x.ExpiredDate)
            .HasColumnType("date");

            base.OnModelCreating(modelBuilder);
        }
    }
}
