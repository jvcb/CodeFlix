using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategories;

namespace CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;

public interface ICreateCategory
{
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
