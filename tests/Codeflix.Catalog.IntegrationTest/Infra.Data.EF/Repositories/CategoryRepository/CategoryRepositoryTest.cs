using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Infra.Data.EF;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Repository = CodeFlix.Infra.Data.EF.Repositories;

namespace Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Insert()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext()).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Get()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        var exampleCategory = _fixture.GetExampleCategory();
        exampleCategoriesList.Add(exampleCategory);

        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext());

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var category = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

        category.Should().NotBeNull();
        category!.Id.Should().Be(exampleCategory.Id);
        category.Name.Should().Be(exampleCategory.Name);
        category.Description.Should().Be(exampleCategory.Description);
        category.IsActive.Should().Be(exampleCategory.IsActive);
        category.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();

        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext());

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        Func<Task<Category>> task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {exampleId} not found");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Update()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValues = _fixture.GetExampleCategory();
        exampleCategoriesList.Add(exampleCategory);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        exampleCategory.Deactivate();

        await categoryRepository.Update(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await (_fixture.CreateDbContext()).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().BeFalse();
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Delete()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        var exampleCategory = _fixture.GetExampleCategory();
        exampleCategoriesList.Add(exampleCategory);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        await categoryRepository.Delete(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await (_fixture.CreateDbContext()).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchReturnsListAndTotal()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValues = _fixture.GetExampleCategory();
        exampleCategoriesList.Add(exampleCategory);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        exampleCategory.Deactivate();

        await categoryRepository.Update(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await (_fixture.CreateDbContext()).Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().BeFalse();
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }
}