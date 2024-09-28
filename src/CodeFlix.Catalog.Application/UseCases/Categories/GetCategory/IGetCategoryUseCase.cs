using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.GetCategory;

public interface IGetCategoryUseCase : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
}
