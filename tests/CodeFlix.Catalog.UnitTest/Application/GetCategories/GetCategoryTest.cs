﻿using CodeFlix.Catalog.Application.UseCases.Categories.GetCategory;
using FluentAssertions;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.GetCategories;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = "")]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(), 
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var input = new GetCategoryInput(exampleCategory.Id);
        var useCase = new GetCategoryUseCase(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }
}
