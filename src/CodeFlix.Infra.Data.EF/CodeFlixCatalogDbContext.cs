using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CodeFlix.Infra.Data.EF;

public class CodeFlixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public CodeFlixCatalogDbContext(
        DbContextOptions<CodeFlixCatalogDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
