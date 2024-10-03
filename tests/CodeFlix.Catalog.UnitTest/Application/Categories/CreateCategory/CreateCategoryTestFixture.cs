using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.UnitTest.Application.Categories.Common;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : CategoryUseCaseFixtureBase
{
    public CreateCategoryInput GetInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name.Substring(0, 2);

        return invalidInputShortName;
    }


    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetInput();
        while (invalidInputTooLongName.Name.Length <= 255)
            invalidInputTooLongName.Name += Faker.Commerce.ProductName();
        invalidInputTooLongName.Name += Faker.Commerce.ProductName();

        return invalidInputTooLongName;
    }

    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var invalidInputDescriptionNull = GetInput();
        invalidInputDescriptionNull.Description = null!;

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputTooLongDescription = GetInput();
        while (invalidInputTooLongDescription.Description.Length <= 10_000)
            invalidInputTooLongDescription.Description += Faker.Commerce.ProductDescription();

        return invalidInputTooLongDescription;
    }
}
