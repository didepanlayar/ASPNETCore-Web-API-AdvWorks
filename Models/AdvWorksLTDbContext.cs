#nullable disable

using AdvWorksAPI.EntityLayer;
using Microsoft.EntityFrameworkCore;

namespace AdvWorksAPI.Models;

public partial class AdvWorksLTDbContext : DbContext
{
    public AdvWorksLTDbContext(DbContextOptions<AdvWorksLTDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
