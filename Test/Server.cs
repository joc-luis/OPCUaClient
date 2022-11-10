using OPCUaClient;

namespace Test
{
    public class Server
    {
        [Test]
        public void Connection()
        {

            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            client.Disconnect();
            Assert.Pass();
        }

        [Test]
        public void FullScan()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            var devices = client.Devices(true);
            Assert.AreEqual(2, devices.Count);
            Assert.AreEqual(2, devices[0].Groups.Count);
            Assert.AreEqual("NexusMeter", devices[0].Name);
            Assert.AreEqual(1022, devices[0].Tags.Count);
        }

        [Test]
        public void Devices()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            var devices = client.Devices();

            Assert.AreEqual(2, devices.Count);
            client.Disconnect();
        }

        [Test]
        public void Groups()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            var groups = client.Groups("NexusMeter", false);

            Assert.AreEqual(2, groups.Count);
            client.Disconnect();
        }


        [Test]
        public void Tags()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);

            var tags = client.Tags("NexusMeter");
            Assert.AreEqual(1022, tags.Count);

            tags = client.Tags("NexusMeter.Test");
            Assert.AreEqual(5, tags.Count);
            
            client.Disconnect();
        }
    }
}
