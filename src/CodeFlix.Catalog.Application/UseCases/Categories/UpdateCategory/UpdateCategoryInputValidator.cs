using FluentValidation;

namespace CodeFlix.Catalog.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
