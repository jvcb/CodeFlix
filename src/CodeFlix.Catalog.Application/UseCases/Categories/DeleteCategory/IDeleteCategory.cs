using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.DeleteCategory;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput, Unit>
{
}
