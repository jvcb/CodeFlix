using CodeFlix.Catalog.Domain.Entities;
using CodeFlix.Catalog.Domain.Exceptions;

namespace CodeFlix.Catalog.UnitTest.Domain.Entities.Categories;

#pragma warning disable S3453
public class CategoryTest
#pragma warning restore S3453
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        var datetimeBefore = DateTime.Now;

        var category = new Category(validData.Name, validData.Description);

        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        var datetimeBefore = DateTime.Now;

        var category = new Category(validData.Name, validData.Description, isActive);

        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        var category = new Category(validData.Name, validData.Description, false);
        category.Activate();


        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        var category = new Category(validData.Name, validData.Description, true);
        category.Deactivate();


        Assert.False(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new Category(name!, "Category Description");

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Name should not be empty or null", ex.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new Category("Category Name", null!);

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Description should not be empty or null", ex.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action = () => new Category(invalidName, "Category Description");

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Name should be at least 3 characters long", ex.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidLongName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action = () => new Category(invalidLongName, "Category Description");

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Name should be less or equal 255 characters long", ex.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var invalidLongDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action = () => new Category("Category Name", invalidLongDescription);

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Description should be less or equal 10_000 characters long", ex.Message);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validData = new { Name = "Category Name", Description = "Category Description" };
        var newValues = new { Name = "New Name", Description = "New Description" };

        var category = new Category(validData.Name, validData.Description);

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validData = new { Name = "Category Name", Description = "Category Description" };
        var newValues = new { Name = "New Name" };

        var category = new Category(validData.Name, validData.Description);

        category.Update(newValues.Name);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validData = new { Name = "Category Name", Description = "Category Description" };
        var category = new Category(validData.Name, validData.Description);

        Action action = () => category.Update(name!);

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Name should not be empty or null", ex.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validData = new { Name = "Category Name", Description = "Category Description" };
        var category = new Category(validData.Name, validData.Description);

        Action action = () => category.Update(invalidName, "Category Description");

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Name should be at least 3 characters long", ex.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var validData = new { Name = "Category Name", Description = "Category Description" };
        var category = new Category(validData.Name, validData.Description);

        var invalidLongName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action = () => category.Update(invalidLongName, "Category Description");

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Name should be less or equal 255 characters long", ex.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validData = new { Name = "Category Name", Description = "Category Description" };
        var category = new Category(validData.Name, validData.Description);

        var invalidLongDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action = () => category.Update("Category Name", invalidLongDescription);

        var ex = Assert.Throws<EntityValidationException>(() => action());

        Assert.Equal("Description should be less or equal 10_000 characters long", ex.Message);
    }
}
