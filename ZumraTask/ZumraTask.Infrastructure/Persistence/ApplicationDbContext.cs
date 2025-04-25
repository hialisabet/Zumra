using Microsoft.EntityFrameworkCore;
using ZumraTask.Domain.Entities;

namespace ZumraTask.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>(eb =>
        {
            eb.HasKey(e => e.Id);
            eb.Property(e => e.Title).IsRequired().HasMaxLength(200);
            eb.Property(e => e.Description).IsRequired();
            eb.Property(e => e.Status).HasConversion<string>().IsRequired();
        });
    }
}