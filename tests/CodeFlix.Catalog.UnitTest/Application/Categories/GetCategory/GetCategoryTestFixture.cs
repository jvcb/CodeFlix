using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.UnitTest.Application.Categories.Common;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection :
    ICollectionFixture<GetCategoryTestFixture>
{ }

public class GetCategoryTestFixture : CategoryUseCaseFixtureBase
{
}
