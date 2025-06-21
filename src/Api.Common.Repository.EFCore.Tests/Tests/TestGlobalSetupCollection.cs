using Xunit;

namespace Api.Common.Repository.EFCoreTests.Tests
{
    [CollectionDefinition("Repository.Tests.Global.Setup")]
    public class TestGlobalSetupCollection : ICollectionFixture<SetupTests>
    {
    }
}