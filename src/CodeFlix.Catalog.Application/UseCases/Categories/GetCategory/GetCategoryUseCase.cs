using CodeFlix.Catalog.Domain.Repositories;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.GetCategory;

public class GetCategoryUseCase : IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetCategoryOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        return GetCategoryOutput.FromCategory(category);
    }
}
