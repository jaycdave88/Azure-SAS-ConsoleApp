using System;
using System.Collections.Generic;

namespace AzurePOC
{
    class ConsoleInterface
    {
        /// <summary>
        /// UserUI will prompt the user for input and then return the List<string>
        /// </summary>
        /// <returns></returns>
        public static List<string> UserUI()
        {
            List<string> accessCredentials = new List<string>();

            Console.WriteLine("Account Name: ");
            string accountName = Console.ReadLine();
            accessCredentials.Add(accountName);
            Console.WriteLine("\n");

            Console.WriteLine("Account Key: ");
            string accountKey = Console.ReadLine();
            accessCredentials.Add(accountKey);
            Console.WriteLine("\n");


            Console.WriteLine("Container Name: ");
            string containerName = Console.ReadLine();
            accessCredentials.Add(containerName);
            Console.WriteLine("\n");

            return accessCredentials;
        }

    }
}
