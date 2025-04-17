using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamyDayWeddingPlanningWeb.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<WeddingTask> WeddingTasks { get; set; }
    public DbSet<Wedding> Weddings { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<TimelineEvent> TimelineEvents { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Wedding>().HasQueryFilter(w => !w.IsDeleted);
        builder.Entity<WeddingTask>().HasQueryFilter(wt => !wt.IsDeleted);
        builder.Entity<Guest>().HasQueryFilter(g => !g.IsDeleted);
        builder.Entity<Budget>().HasQueryFilter(b => !b.IsDeleted);
        builder.Entity<TimelineEvent>().HasQueryFilter(te => !te.IsDeleted);
        builder.Entity<Vendor>().HasQueryFilter(v => !v.IsDeleted);

        // Configure relationships
        builder.Entity<Wedding>()
            .HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(w => w.UserId);

        builder.Entity<Wedding>()
            .HasOne(w => w.Planner)
            .WithMany()
            .HasForeignKey(w => w.PlannerId)
            .IsRequired(false);

        builder.Entity<WeddingTask>()
            .HasOne(wt => wt.User)
            .WithMany()
            .HasForeignKey(wt => wt.UserId);

       

        builder.Entity<Guest>()
            .HasOne(g => g.Wedding)
            .WithMany()
            .HasForeignKey(g => g.WeddingId);

        builder.Entity<Budget>()
            .HasOne(b => b.Wedding)
            .WithMany()
            .HasForeignKey(b => b.WeddingId);

        builder.Entity<TimelineEvent>()
            .HasOne(te => te.Wedding)
            .WithMany()
            .HasForeignKey(te => te.WeddingId);

        builder.Entity<Vendor>()
            .HasOne(v => v.Wedding)
            .WithMany()
            .HasForeignKey(v => v.WeddingId)
            .IsRequired(false);



        builder.Entity<ActivityLog>()
            .HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId);
    }
}

