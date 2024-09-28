using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.UpdateCategory;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
}
