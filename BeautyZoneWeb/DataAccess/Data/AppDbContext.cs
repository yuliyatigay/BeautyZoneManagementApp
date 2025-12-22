using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Procedure> Procedures { get; set; }
    public DbSet<BeautyTech> BeautyTechs { get; set; }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Procedure>().ToTable("Procedures");
        modelBuilder.Entity<BeautyTech>().ToTable("BeautyTechs");
        modelBuilder.Entity<Account>().ToTable("Accounts");

        modelBuilder.Entity<Procedure>()
            .HasMany(m => m.BeautyTechs)
            .WithMany(m => m.Procedures);
        modelBuilder.Entity<Procedure>()
            .HasIndex(p => p.Name)
            .IsUnique();
        modelBuilder.Entity<Customer>()
            .HasIndex(m => m.PhoneNumber)
            .IsUnique();
        modelBuilder.Entity<BeautyTech>()
            .HasIndex(m => m.PhoneNumber)
            .IsUnique();
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.UserName)
            .IsUnique();

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Email)
            .IsUnique();
    }
}