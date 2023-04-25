using Microsoft.EntityFrameworkCore;
using RetAil.Dal.Entities;

namespace RetAil.Dal;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CategoryEntity> CategoryEntities { get; set; }
    public DbSet<ProductEntity> ProductEntities { get; set; }
    
}