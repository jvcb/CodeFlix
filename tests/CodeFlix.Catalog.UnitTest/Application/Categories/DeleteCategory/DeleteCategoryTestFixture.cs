using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.UnitTest.Application.Categories.Common;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.DeleteCategory;

public class DeleteCategoryTestFixture : CategoryUseCaseFixtureBase
{
}

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection
    : ICollectionFixture<DeleteCategoryTestFixture>
{ }

