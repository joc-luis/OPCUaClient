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
            Assert.Equals(true, tag.Value);

            client.Write("NexusMeter.Test.Boolean", false);
            tag = client.Read("NexusMeter.Test.Boolean");
            Assert.Equals(false, tag.Value);

            client.Disconnect();
            
        }

        [Test]
        public void UInteger()
        {
            const string address = "NexusMeter.Test.UInteger";
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            client.Write(address, 12337);
            UInt32 value = client.Read<UInt32>(address);
            client.Disconnect();
            Assert.That(value, Is.EqualTo(12337));
        }
        
        [Test]
        public void Integer()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", false, true);
            client.Connect(30);
            int value = new Random().Next(int.MinValue, int.MaxValue);
            client.Write("NexusMeter.Test.Integer", value);
            var tag = client.Read("NexusMeter.Test.Integer");
            Assert.Equals(value, tag.Value);

            value = new Random().Next(int.MinValue, int.MaxValue);
            client.Write("NexusMeter.Test.Integer", value);
            tag = client.Read("NexusMeter.Test.Integer");
            int read = client.Read<int>("NexusMeter.Test.Integer");
            Assert.Equals(value, tag.Value);
            Assert.Equals(value, read);

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
            Assert.Equals(value, tag.Value);

            value = new Random().NextDouble();
            client.Write("NexusMeter.Test.Double", value);
            tag = client.Read("NexusMeter.Test.Double");
            Assert.Equals(value, tag.Value);

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
            Assert.Equals(value, tag.Value);

            value = new Random().NextSingle();
            client.Write("NexusMeter.Test.Float", value);
            tag = client.Read("NexusMeter.Test.Float");
            Assert.Equals(value, tag.Value);

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
            Assert.Equals(value, tag.Value);

            while (value.Length < 10)
            {
                value += (char)new Random().Next(32, 166);
            }
            client.Write("NexusMeter.Test.String", value);
            tag = client.Read("NexusMeter.Test.String");
            Assert.Equals(value, tag.Value);

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
            Assert.Equals(values.Count, read.Count);

            for (int i = 0; i < read.Count; i++)
            {
                Assert.Equals(values[i].Value, read[i].Value);
            }
            client.Disconnect();
        }
        
        
        
        
         [Test]
        public async Task BoooleanAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);

            await client.WriteAsync("NexusMeter.Test.Boolean", true);
            var tag = await client.ReadAsync("NexusMeter.Test.Boolean");
            Assert.Equals(true, tag.Value);

            await client.WriteAsync("NexusMeter.Test.Boolean", false);
            tag = await client.ReadAsync("NexusMeter.Test.Boolean");
            Assert.Equals(false, tag.Value);

            client.Disconnect();
            
        }

        [Test]
        public async Task UIntegerAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            UInt32 value = await client.ReadAsync<UInt32>("NexusMeter.Test.UInteger");
            client.Disconnect();
            Assert.That(value, Is.EqualTo(12337));
        }
        
        [Test]
        public async Task IntegerAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            int value = new Random().Next(int.MinValue, int.MaxValue);
            await client.WriteAsync("NexusMeter.Test.Integer", value);
            var tag = await client.ReadAsync("NexusMeter.Test.Integer");
            Assert.Equals(value, tag.Value);

            value = new Random().Next(int.MinValue, int.MaxValue);
            client.Write("NexusMeter.Test.Integer", value);
            tag = client.Read("NexusMeter.Test.Integer");
            Assert.Equals(value, tag.Value);

            client.Disconnect();
        }

        [Test]
        public async Task DoubleAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            double value = new Random().NextDouble();
            await client.WriteAsync("NexusMeter.Test.Double", value);
            var tag = await client.ReadAsync("NexusMeter.Test.Double");
            Assert.Equals(value, tag.Value);

            value = new Random().NextDouble();
            await client.WriteAsync("NexusMeter.Test.Double", value);
            tag = await client.ReadAsync("NexusMeter.Test.Double");
            Assert.Equals(value, tag.Value);

            client.Disconnect();
        }

        [Test]
        public async Task FloatAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            float value = new Random().NextSingle();
            await client.WriteAsync("NexusMeter.Test.Float", value);
            var tag = await client.ReadAsync("NexusMeter.Test.Float");
            Assert.Equals(value, tag.Value);

            value = new Random().NextSingle();
            await client.WriteAsync("NexusMeter.Test.Float", value);
            tag = await client.ReadAsync("NexusMeter.Test.Float");
            Assert.Equals(value, tag.Value);

            client.Disconnect();
        }

        [Test]
        public async Task StringAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            client.Connect(30);
            String value = "";

            while (value.Length < 10)
            {
                value += (char)new Random().Next(32, 166);
            }

            await client.WriteAsync("NexusMeter.Test.String", value);
            var tag = await client.ReadAsync("NexusMeter.Test.String");
            Assert.Equals(value, tag.Value);

            while (value.Length < 10)
            {
                value += (char)new Random().Next(32, 166);
            }
            await client.WriteAsync("NexusMeter.Test.String", value);
            tag = await client.ReadAsync("NexusMeter.Test.String");
            Assert.Equals(value, tag.Value);

            client.Disconnect();
        }


        [Test]
        public async Task MultipleAsync()
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

            var result = await client.WriteAsync(values);

            var read = await client.ReadAsync(values.Select(v => v.Address).ToList());
            Assert.Equals(values.Count, read.Count());

            for (int i = 0; i < read.Count(); i++)
            {
                Assert.Equals(values[i].Value, read.ToArray()[i].Value);
            }
            client.Disconnect();
        }

        [Test]
        public async Task MultipleFailAsync()
        {
            UaClient client = new UaClient("testingRead", "opc.tcp://localhost:52240", true, true);
            var values = new List<Tag>();

            for (int i = 0; i < 1000; i++)
            {
                if (i < 10)
                {
                    values.Add(new Tag
                    {
                        Address = $"NexusMeter.Async.Tag00{i}",
                        Value = "Hello"
                    });
                }
                else if(i < 100)
                {
                    values.Add(new Tag
                    {
                        Address = $"NexusMeter.Async.Tag0{i}",
                        Value = "Hello"
                    });
                }
                else
                {
                    values.Add(new Tag
                    {
                        Address = $"NexusMeter.Async.Tag{i}",
                        Value = "Hello"
                    });
                }
            }

            client.Connect();

            var tags = await client.WriteAsync(values);
            client.Disconnect();
            var tag = tags.First(t => t.Name == "Tag989");
            Assert.Equals(tag.Quality, false);
        }
    }
}