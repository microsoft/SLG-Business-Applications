using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace PublicSafety
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // BlobServiceClient bsc = new BlobServiceClient(CredentialsProvider.AzureBlobStorageConnectionString());
            // BlobContainerClient bcc = bsc.GetBlobContainerClient("database");
            // bcc.CreateIfNotExists();

            MockDatabase db = new MockDatabase();
            //db.AddWebhookSubscriptionAsync(new WebhookSubscription()).Wait();
            db.ClearWebhookSubscriptionsAsync().Wait();

        }
    }
}