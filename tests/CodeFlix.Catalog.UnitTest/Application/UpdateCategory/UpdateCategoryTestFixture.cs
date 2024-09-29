using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.UpdateCategory;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.UpdateCategory;


public class UpdateCategoryTestFixture : FixtureBase
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

    public Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());

    public UpdateCategoryInput GetValidInput(Guid? id = null)
        => new (id ?? Guid.NewGuid(),
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
