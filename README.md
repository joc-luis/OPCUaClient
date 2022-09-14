# OPCUaClient
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
 int seconds = 30;
 client.Connect(seconds, true);
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

### Monitoring a tag

```cs
 client.Monitoring("Device.Counter.OK", 500, (_, e) => {
   // Anything you need to be executed when the value changes
 
   // Get the value of the tag being monitored
    var monitored = (MonitoredItemNotification)e.NotificationValue;
    Console.WriteLine(monitored.Value);
```

### Scan OPC UA Server

```cs
 var devices = client.Devices(true);
 
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
 var tags = client.Group("Device.Counter");
 
 foreach(var tag in tags)
 {
    Console.WriteLine($"Name: {tag.Name}");
    Console.WriteLine($"Address: {tag.Address}");
 }
```

### License

[MIT](./LICENSE.md)

Icon for [Freepik](https://www.flaticon.com/authors/freepik)
