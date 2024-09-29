﻿using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Domain.Repositories;

namespace CodeFlix.Catalog.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryUseCase : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryUseCase(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryModelOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        category.Update(request.Name, request.Description);
        
        if (request.IsActive is not null && 
            request.IsActive != category.IsActive)
                if ((bool)request.IsActive!) category.Activate();
                else category.Deactivate();

        await _categoryRepository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
