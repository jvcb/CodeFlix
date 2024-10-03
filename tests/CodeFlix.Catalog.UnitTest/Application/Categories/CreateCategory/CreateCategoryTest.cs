using Bogus;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategory;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Categories.CreateCategory;


[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new CreateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Should().NotBeEquivalentTo(Guid.Empty);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithOnlyName()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new CreateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().BeTrue();
        output.Should().NotBeEquivalentTo(Guid.Empty);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithOnlyNameAndDescription()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new CreateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.Should().NotBeEquivalentTo(Guid.Empty);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(CreateCategoryTestDataGenerator))]
    public void ThrowWhenCantInstantiateAggregate(
        CreateCategoryInput input,
        string exceptionMessage)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new CreateCategoryUseCase(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        Func<Task> action = async () => await useCase.Handle(input, CancellationToken.None);

        action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
