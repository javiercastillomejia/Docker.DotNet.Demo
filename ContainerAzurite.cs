using Docker.DotNet.Models;

namespace Docker.DotNet.Demo
{
    public static class ContainerAzurite
    {
        private const string AzuriteImg = "mcr.microsoft.com/azure-storage/azurite";
        private const string AzuritePrefix = "azurite";
        private const string DockerEmail = "testing@plainconcepts.com";
        private const string DockerUsername = "testing";
        private const string DockerPassword = "P14INK0Nzepts";

        public static async Task<CreateContainerResponse> Start(DockerClient client)
        {
            await client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = AzuriteImg,
                    Tag = "latest",
                },
                new AuthConfig
                {
                    Email = DockerEmail,
                    Username = DockerUsername,
                    Password = DockerPassword
                },
                new Progress<JSONMessage>());

            var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = AzuriteImg,
                Name = $"{AzuritePrefix}-{Guid.NewGuid()}",

                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    {"10002", default}
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {"10002", new List<PortBinding> {new PortBinding {HostPort = "10002" } }}
                    },
                    PublishAllPorts = true
                }
            });

            return response;
        }
    }
}
