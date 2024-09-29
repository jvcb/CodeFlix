using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.ListCategogies;

public interface IListCategories
    : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{

}
