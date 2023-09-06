using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data;

public class ApplicationContext : DbContext
{
    private string? connectionString;
    
    public DbSet<DivineCourse?> DivineCourses { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> contextOptions)
        : base(contextOptions)
    {
        
    }
    
    public ApplicationContext(string? connectionString)
    {
        this.connectionString = connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (connectionString !=null)
        {
            optionsBuilder.UseSqlServer(connectionString);
            
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}