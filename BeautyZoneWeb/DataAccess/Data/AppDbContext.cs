using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Procedure> Procedures { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Procedure>().ToTable("Procedures");
        modelBuilder.Entity<Employee>().ToTable("Employees");

        modelBuilder.Entity<Procedure>()
            .HasMany(p => p.Customers)
            .WithMany(c => c.Procedures);
        modelBuilder.Entity<Procedure>()
            .HasMany(m => m.Employees)
            .WithMany(m => m.Procedures);
        modelBuilder.Entity<Procedure>()
            .HasIndex(p => p.Name)
            .IsUnique();
        modelBuilder.Entity<Customer>()
            .HasIndex(m => m.PhoneNumber)
            .IsUnique();
    }
}