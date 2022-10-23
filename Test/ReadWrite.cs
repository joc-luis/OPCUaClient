using OPCUaClient;
using OPCUaClient.Objects;
namespace Test
{
    public class ReadWrite
    {

        [Test]
        public void Booolean()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);

            client.Write("NexusMeter.Test.Boolean", true);
            var tag = client.Read("NexusMeter.Test.Boolean");
            Assert.AreEqual(true, tag.Value);

            client.Write("NexusMeter.Test.Boolean", false);
            tag = client.Read("NexusMeter.Test.Boolean");
            Assert.AreEqual(false, tag.Value);

            client.Disconnect();
            
        }

        [Test]
        public void Integer()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            int value = new Random().Next(int.MinValue, int.MaxValue);
            client.Write("NexusMeter.Test.Integer", value);
            var tag = client.Read("NexusMeter.Test.Integer");
            Assert.AreEqual(value, tag.Value);

            value = new Random().Next(int.MinValue, int.MaxValue);
            client.Write("NexusMeter.Test.Integer", value);
            tag = client.Read("NexusMeter.Test.Integer");
            Assert.AreEqual(value, tag.Value);

            client.Disconnect();
        }

        [Test]
        public void Double()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            double value = new Random().NextDouble();
            client.Write("NexusMeter.Test.Double", value);
            var tag = client.Read("NexusMeter.Test.Double");
            Assert.AreEqual(value, tag.Value);

            value = new Random().NextDouble();
            client.Write("NexusMeter.Test.Double", value);
            tag = client.Read("NexusMeter.Test.Double");
            Assert.AreEqual(value, tag.Value);

            client.Disconnect();
        }

        [Test]
        public void Float()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            float value = new Random().NextSingle();
            client.Write("NexusMeter.Test.Float", value);
            var tag = client.Read("NexusMeter.Test.Float");
            Assert.AreEqual(value, tag.Value);

            value = new Random().NextSingle();
            client.Write("NexusMeter.Test.Float", value);
            tag = client.Read("NexusMeter.Test.Float");
            Assert.AreEqual(value, tag.Value);

            client.Disconnect();
        }

        [Test]
        public void String()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            String value = "";

            while (value.Length < 10)
            {
                value += (char)new Random().Next(32, 166);
            }

            client.Write("NexusMeter.Test.String", value);
            var tag = client.Read("NexusMeter.Test.String");
            Assert.AreEqual(value, tag.Value);

            while (value.Length < 10)
            {
                value += (char)new Random().Next(32, 166);
            }
            client.Write("NexusMeter.Test.String", value);
            tag = client.Read("NexusMeter.Test.String");
            Assert.AreEqual(value, tag.Value);

            client.Disconnect();
        }


        [Test]
        public void Multiple()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            var values = new List<Tag>
            {
                new Tag
                {
                    Address = "NexusMeter.Test.Boolean",
                    Value = new Random().Next(0, 2) == 1
                },
                new Tag
                {
                    Address = "NexusMeter.Test.Double",
                    Value = new Random().NextDouble()
                },
                new Tag
                {
                    Address = "NexusMeter.Test.Float",
                    Value = new Random().NextSingle()
                },
                new Tag
                {
                    Address = "NexusMeter.Test.Integer",
                    Value = new Random().Next(int.MinValue, int.MaxValue)
                },
                new Tag
                {
                    Address = "NexusMeter.Test.String",
                    Value = "Hello, World!"
                }
            };

            client.Connect();

            client.Write(values);

            var read = client.Read(values.Select(v => v.Address).ToList());
            Assert.AreEqual(values.Count, read.Count);

            for (int i = 0; i < read.Count; i++)
            {
                Assert.AreEqual(values[i].Value, read[i].Value);
            }
            client.Disconnect();
        }
    }
}