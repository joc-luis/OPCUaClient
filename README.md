# OPCUaClient
Client for OPC UA Server


## How to use
### Make a instance

```cs
 UaClient client = new UaClient("test", "opc.tcp://localhost:52240", true, true);
```
### Create a session on the server

```cs
 int seconds = 30;
 client.Connect(seconds);
```


### Close session

```cs
 client.Disconnect();
```


### Read a tag

```cs
 Tag tag = client.Read("Device.Counter.OK");
 Console.WriteLine($"Name: {tag.Name}");
 Console.WriteLine($"Address: {tag.Address}");
 Console.WriteLine($"Value: {tag.Value}");
 Console.WriteLine($"Quality: {tag.Quality}");
```

### Read multiple tags

```cs
var address = new List<String>
{
  "Device.Counter.OK",
  "Device.Counter.NG",
  "Device.Counter.Model",
  "Device.Counter.CycleTime"
}
 var tags = client.Read(address);
 
 foreach(var tag in tags)
 {
    Console.WriteLine($"Name: {tag.Name}");
    Console.WriteLine($"Address: {tag.Address}");
    Console.WriteLine($"Value: {tag.Value}");
    Console.WriteLine($"Quality: {tag.Quality}");
 }
```

### Write a tag

```cs

 client.Write("Device.Counter.Model", "NewModel");
```


### Write multiple tags

```cs
var tags = new List<Tag>
{
  new Tag {
    Address = "Device.Counter.OK",
    Value = 0,
  },
  new Tag {
    Address = "Device.Counter.NG",
    Value = 0,
  },
  new Tag {
    Address = "Device.Counter.Model",
    Value = "OtherModel",
  },
  new Tag {
    Address = "Device.Counter.CycleTime",
    Value = 10,
  },
}
 client.Write(tags);
```

### License

[MIT](./LICENSE.md)

Icon for [Freepik](https://www.flaticon.com/authors/freepik)