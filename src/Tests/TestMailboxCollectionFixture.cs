using IntegrationTests.Tests.Common.Mailpit;

namespace IntegrationTests.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestMailboxCollectionFixture
{
    public TestMailboxCollectionFixture()
    {
        var mailpitApi = new MailpitApi();
        mailpitApi.DeleteMessages().Wait();
    }
}