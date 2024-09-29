
using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Application.UseCases.Categories.ListCategogies;

public class ListCategoriesUseCase : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategoriesUseCase(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await _categoryRepository.Search(
            new SearchInput(
                request.Page, 
                request.PerPage, 
                request.Search, 
                request.Sort, 
                request.Dir), 
            cancellationToken);

        return new ListCategoriesOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(CategoryModelOutput.FromCategory)
                .ToList());
    }
}
