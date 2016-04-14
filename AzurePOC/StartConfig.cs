using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;

namespace AzurePOC
{
    public class StartConfig
    {
        public StartConfig()
        {
            ContainerNames = new List<string>();
        }

        public CloudStorageAccount StorageAccount { get; internal set; }
        public CloudBlobClient BlobClient { get; internal set; }
        public CloudBlobContainer Container { get; internal set; }

        public List<string> ContainerNames { get; internal set; }

        public Tuple<List<string>,List<string>> ListContainerData { get; internal set; }

        public Tuple<string, List<string>> UserInput { get; internal set; }
       
    }
}
