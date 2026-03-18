using CodeFlix.Catalog.Application.Interfaces;

namespace CodeFlix.Infra.Data.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly CodeFlixCatalogDbContext _context;

    public UnitOfWork(CodeFlixCatalogDbContext context)
        => _context = context;

    public async Task Commit(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);
}
