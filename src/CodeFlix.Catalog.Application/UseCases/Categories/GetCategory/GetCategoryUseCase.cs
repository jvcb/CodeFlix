using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Domain.Repositories;

namespace CodeFlix.Catalog.Application.UseCases.Categories.GetCategory;

public class GetCategoryUseCase : IGetCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
