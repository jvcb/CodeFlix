using Bogus;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Infra.Data.EF;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFlix.Catalog.EndToEndTests.Base;

public class BaseFixture
{
    public Faker Faker { get; set; }
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
    }

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription[..10_000];
        return categoryDescription;
    }

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;

    public Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean());

    public List<Category> GetExampleCategoriesList(int length = 15)
        => Enumerable.Range(0, length)
            .Select(_ => GetExampleCategory())
            .ToList();

    public async Task<CodeFlixCatalogDbContext> CreateDbContextAsync()
    {
        var scope = WebAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CodeFlixCatalogDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        return dbContext;
    }

    public async Task CleanPersistenceAsync()
    {
        using var scope = WebAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CodeFlixCatalogDbContext>();
        dbContext.Categories.RemoveRange(dbContext.Categories);
        await dbContext.SaveChangesAsync();
    }
}
