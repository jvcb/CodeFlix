namespace CodeFlix.Catalog.Domain.SeedWork;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}
