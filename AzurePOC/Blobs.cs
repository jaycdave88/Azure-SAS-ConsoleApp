using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurePOC
{
    class Blobs
    {
        /// <summary>
        /// Will list the URL of each blob within the specific Container
        /// </summary>
        public void ListBlobs()
        {
            var containerList = Container.ListContainer(Program.GetConfig(null));

            foreach (var containerName in containerList.Item1)
            {
                var client = Program.GetConfig(null);
                if (client != null)
                {
                    var blobClient = Program.GetConfig(null).BlobClient;
                    var storageAccount = Program.GetConfig(null).StorageAccount;

                    CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                    foreach (IListBlobItem item in container.ListBlobs(null, false))
                    {
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob = (CloudBlockBlob)item;

                            Console.WriteLine("Block blob length: {0} & URL: {1}", blob.Properties.Length, blob.Uri);


                        }
                        else if (item.GetType() == typeof(CloudPageBlob))
                        {
                            CloudPageBlob pageBlob = (CloudPageBlob)item;
                            Console.WriteLine("Page blob length: {0} & URL: {1}", pageBlob.Properties.Length, pageBlob.Uri);
                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            CloudBlobDirectory directory = (CloudBlobDirectory)item;

                            Console.WriteLine("Directory: {0}", directory.Uri);
                        }
                    }
                }
            }
        }

    }
}
