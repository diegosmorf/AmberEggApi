using Xunit;

namespace AmberEggApi.IntegrationTests.Tests;

[CollectionDefinition("Integration.Tests.Global.Setup")]
public class TestGlobalSetupCollection : ICollectionFixture<SetupTests>
{
}
