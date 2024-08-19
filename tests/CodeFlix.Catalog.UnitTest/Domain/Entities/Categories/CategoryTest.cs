using CodeFlix.Catalog.Domain.Entities;

namespace CodeFlix.Catalog.UnitTest.Domain.Entities.Categories;

public class CategoryTest
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

        var category = new Category(validData.Name, validData.Description);
    
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
    }
}
