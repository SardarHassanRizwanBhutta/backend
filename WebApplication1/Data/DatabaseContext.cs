using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

// Main class for coordinating Entity Framework functionality for a data model
// Dependency object is a object on which other objects depend upon
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    // Db set properties for each entity in the model
    // DbSet<TEntity>
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Role> Roles {get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seeding predefined roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "SuperAdmin" },
            new Role { Id = 2, Name = "Admin" },
            new Role { Id = 3, Name = "User" }
        );
    }
}