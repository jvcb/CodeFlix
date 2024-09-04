using Bogus;

namespace CodeFlix.Catalog.UnitTest.Common;

public abstract class FixtureBase
{
    public Faker Faker { get; set; }

    protected FixtureBase()
       => Faker = new Faker("pt_BR");
}
