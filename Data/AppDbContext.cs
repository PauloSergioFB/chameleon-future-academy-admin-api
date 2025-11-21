using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<CourseTag> CourseTags => Set<CourseTag>();
    public DbSet<Content> Contents => Set<Content>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ActivityOption> ActivityOptions => Set<ActivityOption>();
    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CourseTag>()
            .HasKey(ct => new { ct.CourseId, ct.TagId });

        modelBuilder.Entity<CourseTag>()
            .HasOne(ct => ct.Course)
            .WithMany(c => c.CourseTags)
            .HasForeignKey(ct => ct.CourseId);

        modelBuilder.Entity<CourseTag>()
            .HasOne(ct => ct.Tag)
            .WithMany(t => t.CourseTags)
            .HasForeignKey(ct => ct.TagId);
    }

}