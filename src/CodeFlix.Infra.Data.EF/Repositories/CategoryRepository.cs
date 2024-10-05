using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Infra.Data.EF.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CodeFlixCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(CodeFlixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        => await _categories.AddAsync(aggregate, cancellationToken);

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_categories.Remove(aggregate));
    }

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.AsNoTracking().FirstOrDefaultAsync((x => x.Id == id), cancellationToken);

        NotFoundException.ThrowIfNull(category, $"Category {id} not found");

        return category!;
    }

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_categories.Update(aggregate));
    }
}
