using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.SeedWork;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Domain.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
{

}
