# [OPCUaClient](https://www.nuget.org/packages/OPCUaClient/)
Client for OPC UA Server

## Build with
[OPC UA Foundation](https://github.com/OPCFoundation/UA-.NETStandard) library

## Certificates
The certificates are in the same folder the application executable.

## How to use
### Install
```
dotnet add package OPCUaClient
```
### Import

```cs
 using OPCUaClient;
```



### Create a instance

```cs
 UaClient client = new UaClient("test", "opc.tcp://localhost:52240", true, true);
 
 UaClient auth = new UaClient("test", "opc.tcp://localhost:52240", true, true, "admin", "password");
```
### Create a session on the server

```cs
 int timeOut = 30;
 client.Connect(timeOut, true);
```


### Close session

```cs
 client.Disconnect();
```


### Change the namespace index (default namespace index is 2)

```cs
// Fixed namespace index
client.NamespaceIndex = 3;

// From URI
client.SetNamespaceIndexFromUri("urn://N44/Ua/Device1");

// From identifier 
client.SetNamespaceIndexFromIdentifier("ua-server-identifier");

```


### Read a tag

```cs
 Tag tag = client.Read("Device.Counter.OK");
 //Or
 tag = await client.Read("Device.Counter.OK");
 
 Console.WriteLine($"Name: {tag.Name}");
 Console.WriteLine($"Address: {tag.Address}");
 Console.WriteLine($"Value: {tag.Value}");
 Console.WriteLine($"Quality: {tag.Quality}");
 Console.WriteLine($"Code: {tag.Code}");
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
 //Or
 tags await = client.ReadAsync(address);
 
 foreach(var tag in tags)
 {
    Console.WriteLine($"Name: {tag.Name}");
    Console.WriteLine($"Address: {tag.Address}");
    Console.WriteLine($"Value: {tag.Value}");
    Console.WriteLine($"Quality: {tag.Quality}");
    Console.WriteLine($"Code: {tag.Code}");
 }
```

### Write a tag

```cs
 client.Write("Device.Counter.Model", "NewModel");
 //Or
 await client.WriteAsync("Device.Counter.Model", "NewModel");
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
//Or
 await client.WriteAsync(tags);
```

### Monitoring a tag

```cs
 client.Monitoring("Device.Counter.OK", 500, (_, e) => {
   // Anything you need to be executed when the value changes
 
   // Get the value of the tag being monitored
    var monitored = (MonitoredItemNotification)e.NotificationValue;
    Console.WriteLine(monitored.Value);
 });
```

### Scan OPC UA Server

```cs
 var devices = client.Devices(true);
 //Or
 devices = await client.DevicesAsync(true);
 
 foreach(var device in devices)
 {
    Console.WriteLine($"Name: {device.Name}");
    Console.WriteLine($"Address: {device.Address}");
    Console.WriteLine($"Groups: {device.Groups.Count()}");
    Console.WriteLine($"Tags: {device.Tags.Count()}");
 }
```

### Scan group

```cs
 var groups = client.Group("Device", true);
 //Or
groups = await client.GroupAsync("Device", true); 
 
 foreach(var group in groups)
 {
    Console.WriteLine($"Name: {group.Name}");
    Console.WriteLine($"Address: {group.Address}");
    Console.WriteLine($"Groups: {group.Groups.Count()}");
    Console.WriteLine($"Tags: {group.Tags.Count()}");
 }
```

### Scan an address and recovery the tags

```cs
 var tags = client.Tags("Device.Counter");
 //Or
 tags = await client.TagsAsync("Device.Counter");
 
 foreach(var tag in tags)
 {
    Console.WriteLine($"Name: {tag.Name}");
    Console.WriteLine($"Address: {tag.Address}");
 }
```

### License

[MIT](./LICENSE.md)

Icon for [Freepik](https://www.flaticon.com/authors/freepik)
