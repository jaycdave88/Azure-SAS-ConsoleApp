using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;


namespace AzurePOC
{
    class Container
    {
        /// <summary>
        /// Will return a list of all the containers
        /// </summary>
        /// <returns></returns>
        public static Tuple<List<string>, List<string>> ListContainer(StartConfig config)
        {
            if (config != null || config.BlobClient != null)
            {
                if (config.ListContainerData != null && config.ListContainerData == null)
                {
                    config = Program.GetConfig(null);
                }
            }
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (config.BlobClient == null)
            {
                throw new ArgumentException("BlobClient must not be null", "config");
            }

            List<string> container = new List<string>();

            //Get the list of the blob from the above container
            IEnumerable<CloudBlobContainer> containers = config.BlobClient.ListContainers();

            if (config != null)
            {
                foreach (CloudBlobContainer item in containers)
                {
                    container.Add(item.Name);

                    config.ContainerNames.Add(item.Name);
                }
                config.ListContainerData = new Tuple<List<string>, List<string>>(new List<string>(), new List<string>());
                config.ListContainerData.Item2.AddRange(config.ContainerNames);
                config.ListContainerData.Item1.AddRange(container);
            }

            ///Uncomment to see a list of Container Names

            //Console.WriteLine(String.Join("\n", container));
            //Console.WriteLine("\n");

            return config.ListContainerData;
        }
    }
}
