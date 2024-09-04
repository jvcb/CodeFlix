﻿using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.UnitTest.Common;

namespace CodeFlix.Catalog.UnitTest.Domain.Entities.Categories;

public class CategoryTestFixture : FixtureBase
{
    public CategoryTestFixture()
        : base()
    { }

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

    public Category GetValidCategory()
        => new(GetValidCategoryName(), GetValidCategoryDescription());
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CategoryTestFixture>
{

}