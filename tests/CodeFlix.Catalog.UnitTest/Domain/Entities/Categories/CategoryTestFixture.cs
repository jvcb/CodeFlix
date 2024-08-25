using CodeFlix.Catalog.Domain.Entities;

namespace CodeFlix.Catalog.UnitTest.Domain.Entities.Categories;

public class CategoryTestFixture
{
    public Category GetValidCategory()
        => new("Category Name", "Category Description");
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CategoryTestFixture>
{

}