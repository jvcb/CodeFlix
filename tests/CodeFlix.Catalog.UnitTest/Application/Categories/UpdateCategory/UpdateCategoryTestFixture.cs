using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.UpdateCategory;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.UnitTest.Application.Categories.Common;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.UpdateCategory;


public class UpdateCategoryTestFixture : CategoryUseCaseFixtureBase
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
        => new(id ?? Guid.NewGuid(),
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean());

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name =
            invalidInputShortName.Name.Substring(0, 2);

        return invalidInputShortName;
    }


    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetValidInput();
        while (invalidInputTooLongName.Name.Length <= 255)
            invalidInputTooLongName.Name += Faker.Commerce.ProductName();
        invalidInputTooLongName.Name += Faker.Commerce.ProductName();

        return invalidInputTooLongName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetValidInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();

        while (tooLongDescriptionForCategory.Length <= 10_000)
            tooLongDescriptionForCategory += Faker.Commerce.ProductDescription();

        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;

        return invalidInputTooLongDescription;
    }
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection
    : ICollectionFixture<UpdateCategoryTestFixture>
{ }
