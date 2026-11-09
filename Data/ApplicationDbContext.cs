using CodingPortfolio.Models;
using Microsoft.EntityFrameworkCore;

namespace CodingPortfolio.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Developer>()
                .HasKey(Developer => Developer.DeveloperId);
            modelBuilder.Entity<Developer>()
               .HasMany(developer => developer.Projects)
               .WithOne(project => project.Developer)
               .HasForeignKey(project => project.DeveloperId);
            modelBuilder.Entity<Developer>()
           .Property(d => d.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Project>()
                .HasKey(project => project.ProjectId);
            modelBuilder.Entity<Project>()
                .HasMany(project => project.Tasks)
                .WithOne(task => task.Project)
                .HasForeignKey(task => task.ProjectId);

            modelBuilder.Entity<ProjectTask>()
                .HasKey(task => task.TaskId);



            modelBuilder.Entity<Developer>().HasIndex(Developer => Developer.Email).IsUnique();

        }
    }
}
