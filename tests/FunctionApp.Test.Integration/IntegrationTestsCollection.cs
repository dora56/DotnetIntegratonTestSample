namespace FunctionApp.Test;

[CollectionDefinition(Name)]
public class IntegrationTestsCollection: ICollectionFixture<TestsInitializer>
{
    public const string Name = nameof(IntegrationTestsCollection);
}
