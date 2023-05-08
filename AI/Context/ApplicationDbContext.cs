using AI.Models;
using Microsoft.EntityFrameworkCore;

namespace AI.Context;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }
    public DbSet<AIType> setAI { get; set; }
}