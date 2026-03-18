using CodeFlix.Catalog.EndToEndTests.Base;

namespace CodeFlix.Catalog.EndToEndTests.Api.Categories;

[CollectionDefinition(nameof(CategoryApiTestFixture))]
public class CategoryApiTestFixtureCollection
    : ICollectionFixture<CategoryApiTestFixture>
{ }

public class CategoryApiTestFixture : BaseFixture
{
    public CategoryApiTestFixture() : base() { }
}
