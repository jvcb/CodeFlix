using Bogus;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategories;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.UnitTest.Application.CreateCategory;
using FluentAssertions;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.CreateCategories;


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
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
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
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
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
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
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

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryTestFixture();

        var invalidInputList = new List<object[]>();

        var invalidInputShortName = fixture.GetInput();
        invalidInputShortName.Name.Substring(0, 2);

        invalidInputList.Add(new object[]
        {
            invalidInputShortName,
            "Name should be at least 3 characters long"
        });

        var invalidInputTooLongName = fixture.GetInput();
        while (invalidInputTooLongName.Name.Length <= 255)
            invalidInputTooLongName.Name += fixture.Faker.Commerce.ProductName();

        invalidInputList.Add(new object[]
        {
            invalidInputTooLongName,
            "Name should be less or equal 255 characters long"
        });

        var invalidInputDescriptionNull = fixture.GetInput();
        invalidInputDescriptionNull.Description = null!;

        invalidInputList.Add(new object[]
        {
            invalidInputDescriptionNull,
            "Description should not be null"
        });

        var invalidInputTooLongDescription = fixture.GetInput();
        while (invalidInputTooLongDescription.Description.Length <= 10_000)
            invalidInputTooLongDescription.Description += fixture.Faker.Commerce.ProductDescription();

        invalidInputList.Add(new object[]
        {
            invalidInputTooLongDescription,
            "Description should be less or equal 10000 characters long"
        });

        return invalidInputList;
    }
}
