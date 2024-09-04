using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace CodeFlix.Catalog.UnitTest.Domain.Entities.Categories;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTextFixture;

    public CategoryTest(CategoryTestFixture categoryTextFixture)
        => _categoryTextFixture = categoryTextFixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        var category = new Category(validCategory.Name, validCategory.Description);

        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(Guid.Empty);
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.IsActive.Should().BeTrue();
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTextFixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;

        var category = new Category(validCategory.Name, validCategory.Description, isActive);

        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(Guid.Empty);
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.IsActive.Should().Be(isActive);
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new Category(name!, "Category Description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new Category("Category Name", null!);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategoryDescription = _categoryTextFixture.GetValidCategoryDescription();

        Action action = () => new Category(invalidName, validCategoryDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();

        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;

            yield return new object[] 
            {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };
        }

        yield return new object[] { "1" };
        yield return new object[] { "12" };
        yield return new object[] { "a" };
        yield return new object[] { "ab" };
        yield return new object[] { "ux" };
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidLongName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action = () => new Category(invalidLongName, "Category Description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var invalidLongDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action = () => new Category("Category Name", invalidLongDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10_000 characters long");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var categoryWithNewValues = _categoryTextFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var newName = _categoryTextFixture.GetValidCategoryName();

        var category = new Category(validCategory.Name, validCategory.Description);

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(validCategory.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var category = new Category(validCategory.Name, validCategory.Description);

        Action action = () => category.Update(name!);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var category = new Category(validCategory.Name, validCategory.Description);

        Action action = () => category.Update(invalidName, "Category Description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var category = new Category(validCategory.Name, validCategory.Description);

        var invalidLongName = _categoryTextFixture.Faker.Lorem.Letter(256);

        Action action = () => category.Update(invalidLongName, "Category Description");

        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTextFixture.GetValidCategory();
        var category = new Category(validCategory.Name, validCategory.Description);

        var invalidLongDescription = _categoryTextFixture.Faker.Lorem.Letter(10_001);

        Action action = () => category.Update("Category Name", invalidLongDescription);

        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10_000 characters long");
    }
}
