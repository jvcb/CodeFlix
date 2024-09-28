using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategories;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;

public interface ICreateCategory 
    : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
}
