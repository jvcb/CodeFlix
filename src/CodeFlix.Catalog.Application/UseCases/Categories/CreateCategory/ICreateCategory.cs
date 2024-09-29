using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;

public interface ICreateCategory 
    : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
}
