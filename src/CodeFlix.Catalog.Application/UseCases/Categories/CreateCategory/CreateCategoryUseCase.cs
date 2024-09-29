using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;

namespace CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryUseCase : ICreateCategory
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryUseCase(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new Category(
            input.Name,
            input.Description,
            input.IsActive);

        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
