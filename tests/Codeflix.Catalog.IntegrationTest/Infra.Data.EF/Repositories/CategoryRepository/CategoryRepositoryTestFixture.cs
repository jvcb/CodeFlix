using Codeflix.Catalog.IntegrationTest.Common;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository;

public class CategoryRepositoryTestFixture : FixtureBase
{
    public CodeFlixCatalogDbContext CreateDbContext()
        => new CodeFlixCatalogDbContext(
            new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                    .Options);

    public Category GetExampleCategory()
        => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());


    public List<Category> GetExampleCategoriesList(int length = 10)
        => Enumerable.Range(1, length).Select(_ => GetExampleCategory()).ToList();

    public string GetValidCategoryName()
    {
        var categoryName = string.Empty;

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

    public bool GetRandomBoolean()
    {
        return new Random().NextDouble() < 0.5;
    }
}

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection
    : ICollectionFixture<CategoryRepositoryTestFixture>
{
    
}
