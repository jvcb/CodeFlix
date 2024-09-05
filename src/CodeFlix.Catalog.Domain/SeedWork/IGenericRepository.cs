using CodeFlix.Catalog.Domain.Entities;

namespace CodeFlix.Catalog.Domain.SeedWork;

public interface IGenericRepository<in TAggregate> : IRepository
{
    Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
}
