using OPCUaClient;
using OPCUaClient.Objects;
namespace Test
{
    public class Tests
    {
       
        [SetUp]
        public void Setup()
        {
            UaClient client = new UaClient("testing", "opc.tcp://localhost:52240", true);
        }

        [Test]
        public void Connection()
        {

            UaClient client = new UaClient("testing", "opc.tcp://localhost:52240", true);
            client.Connect(30);
            client.Disconnect();
            Assert.Pass();
        }

        [Test]
        public void Read()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true);
            client.Connect(30);
            Tag tag = client.Read("NexusMeter.Tag");
            client.Disconnect();
            Assert.AreEqual(12337, tag.Value);
        }

        [Test]
        public void ReadList()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true);
            client.Connect(30);
            var address = new List<String>
            {
                "NexusMeter.Tag",
                "NexusMeter.Tag1",
                "NexusMeter.Tag2"
            };
            var tags = client.Read(address);
            Assert.IsNotNull(tags);
            client.Disconnect();

        }
    }
}