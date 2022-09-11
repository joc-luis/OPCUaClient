using OPCUaClient;
using OPCUaClient.Objects;
namespace Test
{
    public class Tests
    {
       
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Connection()
        {

            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            client.Disconnect();
            Assert.Pass();
        }

        [Test]
        public void Read()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            Tag tag = client.Read("NexusMeter.Tag000");
            client.Disconnect();
            Assert.AreEqual(12337, tag.Value);
        }

        [Test]
        public void ReadList()
        {
            UaClient client = new UaClient("testingReadList", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            var address = new List<String>();
            for (int i = 0; i < 1000; i++)
            {
                if (i <= 9)
                {
                    address.Add($"Tag00{i}");
                }
                else if(i <= 99)
                {
                    address.Add($"Tag0{i}");
                }
                else
                {
                    address.Add($"Tag{i}");
                }
            }
            var tags = client.Read(address);
            Assert.AreEqual(1000, tags.Count);
            client.Disconnect();

        }
    }
}