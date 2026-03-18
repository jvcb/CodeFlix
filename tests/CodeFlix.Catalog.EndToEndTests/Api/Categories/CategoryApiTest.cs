using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CodeFlix.Catalog.EndToEndTests.Api.Categories;

[Collection(nameof(CategoryApiTestFixture))]
public class CategoryApiTest : IAsyncLifetime
{
    private readonly CategoryApiTestFixture _fixture;

    public CategoryApiTest(CategoryApiTestFixture fixture)
        => _fixture = fixture;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
        => await _fixture.CleanPersistenceAsync();

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task CreateCategory()
    {
        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription(),
            _fixture.GetRandomBoolean());

        var response = await _fixture.HttpClient.PostAsJsonAsync("/categories", input);

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var output = await response.Content.ReadFromJsonAsync<CategoryModelOutput>();

        output.Should().NotBeNull();
        output!.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task CreateCategoryWithOnlyName()
    {
        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var response = await _fixture.HttpClient.PostAsJsonAsync("/categories", input);

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var output = await response.Content.ReadFromJsonAsync<CategoryModelOutput>();

        output.Should().NotBeNull();
        output!.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(ErrorWhenNameIsEmpty))]
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task ErrorWhenNameIsEmpty()
    {
        var input = new CreateCategoryInput("");

        var response = await _fixture.HttpClient.PostAsJsonAsync("/categories", input);

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("One or more validation errors occurred");
        problemDetails.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        problemDetails.Detail.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task GetCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        var dbContext = await _fixture.CreateDbContextAsync();
        await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();

        var response = await _fixture.HttpClient.GetAsync($"/categories/{exampleCategory.Id}");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var output = await response.Content.ReadFromJsonAsync<CategoryModelOutput>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(ErrorWhenCategoryNotFound))]
    [Trait("EndToEnd/API", "Category - Endpoints")]
    public async Task ErrorWhenCategoryNotFound()
    {
        var randomId = Guid.NewGuid();

        var response = await _fixture.HttpClient.GetAsync($"/categories/{randomId}");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("Not found");
        problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Detail.Should().Be($"Category {randomId} not found");
    }
}
