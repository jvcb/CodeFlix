using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Categories.DeleteCategory;

public class DeleteCategoryInput : IRequest<Unit>
{
    public Guid Id { get; set; }

    public DeleteCategoryInput(Guid id) => Id = id;
}
