using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using System.Collections.Generic;

namespace AzurePOC
{
    class Program
    {

        static void Main(string[] args)
        {
            StartConfig config = GetConfig(ApplicatonStartFromUserOptions(ConsoleInterface.UserUI()));

            //ListContainer(config);

            //Program p = new Program();
            //p.ListBlobs();
        }


        /// <summary>
        /// ApplicationStartFromUserOptions will accept the accessCredentials from UserUI and create a session called connectionString.
        /// A new List<string> will be created to hold the complete connectionString and the pervious accessCredenitals from UserUI
        /// </summary>
        /// <param name="accessCredentials"></param>
        /// <returns></returns>
        public static List<string> ApplicatonStartFromUserOptions(List<string> accessCredentials)
        {
            List<string> userOptions = new List<string>();

            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accessCredentials[0]};AccountKey={accessCredentials[1]}";

            userOptions.Add(connectionString);
            userOptions.AddRange(accessCredentials);

            return userOptions;
        }

        /// <summary>
        /// Start Config will pull the required data and provide it to other methods 
        /// </summary>
        /// <returns></returns>
        public static StartConfig GetConfig(List<string> userInput)
        {
           
            var config = new StartConfig();

            // Retrieve storage account from connection string.
            config.StorageAccount = CloudStorageAccount.Parse(userInput[0]);

            // Create the blob object.
            config.BlobClient = config.StorageAccount.CreateCloudBlobClient();

            config.ListContainerData = Container.ListContainer(config);

            //Check to see if the container was found.
            foreach (var item in config.ListContainerData.Item1)
            {
                if (userInput[3] == item)
                {
                    Console.WriteLine("Container: {0} found!", item);
                }
            }

            //Getting reference to container.
            config.Container = config.BlobClient.GetContainerReference(userInput[3]);

            //Generating a SAS for the container found
           ShowSasTokenForContainer(config);

            //If not  container is found then create one.
            config.Container.CreateIfNotExists();

            return config;
        }

        /// <summary>
        /// Shows SAS token for containers 
        /// </summary>
        static void ShowSasTokenForContainer(StartConfig config)
        {
            // Create a new access policy on the container, which may be optionally used to provide constraints
            // shared access signatures on the container and the blob.
            string sharedAccessPolicyName = CloudConfigurationManager.GetSetting("SasPolicyName");

            //generate a SAS token for the container, using a used acces policy to set constraints on the SAS.
            CreateSharedAccessPolicy(config, sharedAccessPolicyName);

            //Print statement for the Container's SAS
            Console.WriteLine("Container's SAS Token: {0}\r\n", GetContainersSasTokenWithPolicy(config, sharedAccessPolicyName));
        }

        /// <summary>
        /// Creates a new Shared Access Policy for the container.
        /// </summary>
        /// <param name="blobClient"></param>
        /// <param name="container"></param>
        /// <param name="policyName"></param>
        static void CreateSharedAccessPolicy(StartConfig properties, string policyName)
        {
            //creates a new shared access policy and define its constraints.
            SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
            {
                // policy expiration date
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1),
                // policy permissions
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Create
            };

            //Fetch the container's existing permissions.
            BlobContainerPermissions permissions = properties.Container.GetPermissions();

            //Add new policy to the container's permissions & get container's permissions
            if (permissions.SharedAccessPolicies.Equals(null))
            {
                permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
                properties.Container.SetPermissions(permissions);
            }

            permissions.SharedAccessPolicies.Remove(policyName);
        }

        /// <summary>
        /// Gets the SharedAccessSignature token from the continer, using the specifed policy
        /// </summary>
        /// <param name="container"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        static string GetContainersSasTokenWithPolicy(StartConfig properties, string policyName)
        {
            //generate the sahred access signature on the container. In 
            string sasContainerToken = properties.Container.GetSharedAccessSignature(null, policyName);

            //return the SAS token'
            return sasContainerToken;
        }







    }


}
