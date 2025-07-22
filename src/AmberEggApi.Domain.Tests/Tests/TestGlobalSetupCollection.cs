using Xunit;

namespace AmberEggApi.DomainTests.Tests;

[CollectionDefinition("Domain.Tests.Global.Setup")]
public class TestGlobalSetupCollection : ICollectionFixture<SetupTests>
{
}