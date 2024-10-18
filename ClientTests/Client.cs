using TestClientProject;

namespace ClientTests
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void BaseTest()
        {
            Client client = new Client();

            Assert.IsNotNull(client);
        }
    }
}