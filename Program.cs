using System.Text.Json;
using Docker.DotNet;
using Docker.DotNet.Demo;
using Docker.DotNet.Models;

Console.WriteLine("Hola, Team Melee & Rocket!");
Console.WriteLine("Vamos a crear un contendor con el emulador de Azurite");
Console.ReadLine();

// Init Container 
var dockerClient = new DockerClientConfiguration().CreateClient();
var responseContainer = await ContainerAzurite.Start(dockerClient);
await dockerClient.Containers.StartContainerAsync(responseContainer.ID, null);

Console.WriteLine("Container OK!");
Console.ReadLine();

// Insert 
Console.WriteLine("Create record");

var storage = new Storage();
var idRecord = Guid.NewGuid().ToString("N");
await storage.Create(new OurRecord(idRecord, idRecord, "Team Melee & Rocket"));

Console.WriteLine("Create record OK!");
Console.ReadLine();

Console.WriteLine("Read record");
var ourRecord = await storage.Read(idRecord);
Console.WriteLine("Read record OK!");
Console.WriteLine(JsonSerializer.Serialize(ourRecord));
Console.ReadLine();

// Delete Container
var stopParams = new ContainerStopParameters()
{
    WaitBeforeKillSeconds = 3
};
await dockerClient.Containers.StopContainerAsync(responseContainer.ID, stopParams, CancellationToken.None);

var removeParams = new ContainerRemoveParameters()
{
    Force = true
};
await dockerClient.Containers.RemoveContainerAsync(responseContainer.ID, removeParams, CancellationToken.None);
dockerClient.Dispose();

Console.WriteLine("Delete Container OK!");
Console.ReadLine();