using CodeFlix.Catalog.Domain.Entities;
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

        var useCase = new CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            "Category Name",
            "Category Description",
            true);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Create(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once);

        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()), 
            Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Name");
        output.IsActive.Should().BeTrue();
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
    }
}
