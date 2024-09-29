using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection 
    : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : FixtureBase
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidCategoryName()
    {
        var categoryName = String.Empty;

        while (categoryName.Length < 3) categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255) categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000) categoryDescription = categoryDescription[..10_000];

        return categoryDescription;
    }

    public bool GetRandomBoolean() => (new Random()).NextDouble() < 0.5;

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
