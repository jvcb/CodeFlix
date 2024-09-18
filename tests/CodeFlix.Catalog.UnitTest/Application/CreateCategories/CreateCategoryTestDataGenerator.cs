using CodeFlix.Catalog.UnitTest.Application.CreateCategory;

namespace CodeFlix.Catalog.UnitTest.Application.CreateCategories;

public static class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputList.Add(
                    [
                        fixture.GetInvalidInputShortName(),
                        "Name should be at least 3 characters long"
                    ]);
                    break;
                case 1:
                    invalidInputList.Add(
                    [
                        fixture.GetInvalidInputTooLongName(),
                        "Name should be less or equal 255 characters long"
                    ]);
                    break;
                case 2:
                    invalidInputList.Add(
                    [
                        fixture.GetInvalidInputDescriptionNull(),
                        "Description should not be null"
                    ]);
                    break;
                case 3:
                    invalidInputList.Add(
                    [
                        fixture.GetInvalidInputTooLongDescription(),
                        "Description should be less or equal 10000 characters long"
                    ]);
                    break;
                default:
                    break;
            }
        }

        return invalidInputList;
    }
}
