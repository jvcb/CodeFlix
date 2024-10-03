using CodeFlix.Catalog.Application.UseCases.Categories.ListCategogies;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.UnitTest.Application.Categories.Common;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.ListCategories;

public class ListCategoriesTestFixture : CategoryUseCaseFixtureBase
{
    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<Category>();

        for (int i = 0; i < length; i++)
        {
            list.Add(GetExampleCategory());
        }

        return list;
    }

    public ListCategoriesInput GetExampleInput()
    {
        var random = new Random();

        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc);
    }
}

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection
    : ICollectionFixture<ListCategoriesTestFixture>
{ }
