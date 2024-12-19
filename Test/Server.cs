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
            Assert.Equals(2, devices.Count);
            Assert.Equals(3, devices[0].Groups.Count);
            Assert.Equals("NexusMeter", devices[0].Name);
            Assert.Equals(1022, devices[0].Tags.Count);
        }

        [Test]
        public void Devices()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            var devices = client.Devices();

            Assert.Equals(2, devices.Count);
            client.Disconnect();
        }

        [Test]
        public void Groups()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            var groups = client.Groups("NexusMeter", false);

            Assert.Equals(3, groups.Count);
            client.Disconnect();
        }


        [Test]
        public void Tags()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);

            var tags = client.Tags("NexusMeter");
            Assert.Equals(1022, tags.Count);

            tags = client.Tags("NexusMeter.Test");
            Assert.Equals(6, tags.Count);
            
            client.Disconnect();
        }
        
       

        
        [Test]
        public async Task DevicesAsync()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            await client.ConnectAsync(30);
            var devices =  client.DevicesAsync();

            Assert.Equals(2, (await devices).Count);
            await client.DisconnectAsync();
        }

        [Test]
        public async Task GroupsAsync()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            await client.ConnectAsync(30);
            var groups = await client.GroupsAsync("NexusMeter", false);

            Assert.Equals(3, groups.Count);
            await client.DisconnectAsync();
        }

        [Test]
        public async Task TagsAsync()
        {
            UaClient client = new UaClient("testingConect", "opc.tcp://localhost:52240", true, true);
            await client.ConnectAsync(30);
            
            var tags = await client.TagsAsync("NexusMeter.Test");
            Assert.Equals(6, tags.Count);

            tags = await client.TagsAsync("NexusMeter");
            Assert.Equals(1044, tags.Count);
            
            await client.DisconnectAsync();
        }
    }
}
