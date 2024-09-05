using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Categories.CreateCategories;
using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.CreateCategories;

public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var useCase = new CreateCategoryUseCase(
            repositoryMock.Object, 
            unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            "Category Name",
            "Category Description",
            true);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()), 
            Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Description");
        output.IsActive.Should().BeTrue();
        output.Should().NotBeEquivalentTo(Guid.Empty);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }
}
