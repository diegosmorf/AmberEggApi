using Xunit;

namespace AmberEggApi.Repository.EFCoreTests.Tests;

[CollectionDefinition("Repository.Tests.Global.Setup")]
public class TestGlobalSetupCollection : ICollectionFixture<SetupTests>
{
}