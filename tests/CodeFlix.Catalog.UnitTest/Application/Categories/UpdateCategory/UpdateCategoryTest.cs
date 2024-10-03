using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Categories.Common;
using CodeFlix.Catalog.Application.UseCases.Categories.UpdateCategory;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateCategory))]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator))]
    [Trait("Application", "UpdateCategory - UseCases")]
    public async Task UpdateCategory(Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(exampleInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleInput.Name);
        output.Description.Should().Be(exampleInput.Description);
        output.IsActive.Should().Be((bool)exampleInput.IsActive!);

        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowCategoryWhenNotFound))]
    [Trait("Application", "UpdateCategory - UseCases")]
    public async Task ThrowCategoryWhenNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var exampleInput = _fixture.GetValidInput();

        repositoryMock.Setup(x => x.Get(exampleInput.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{exampleInput.Id}' not found"));

        var useCase = new UpdateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async () => await useCase.Handle(exampleInput, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(exampleInput.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [MemberData(
    nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
    parameters: 10,
    MemberType = typeof(UpdateCategoryTestDataGenerator))]
    [Trait("Application", "UpdateCategory - UseCases")]
    public async Task UpdateCategoryWithoutProvidingIsActive(
        Category exampleCategory,
        UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.Description);

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);

        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategorOnlyName))]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator))]
    [Trait("Application", "UpdateCategory - UseCases")]
    public async Task UpdateCategorOnlyName(
        Category exampleCategory,
        UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name);

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);

        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task ThrowWhenCantUpdateCategory(UpdateCategoryInput input, string expectedExceptionMessage)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleCategory = _fixture.GetExampleCategory();
        input.Id = exampleCategory.Id;

        repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var useCase = new UpdateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);

        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
