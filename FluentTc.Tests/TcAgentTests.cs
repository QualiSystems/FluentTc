using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class TcAgentTests
    {
        [Test]
        public void GetAllAgents()
        {
            var tcAgents = new RemoteTc().Connect(_ => _.ToHost("tc").AsUser("igor.o", "Quali2222!"))
                                    .GetAgents(h => h.Connected().Authorized().Enabled());

        }
    }
}