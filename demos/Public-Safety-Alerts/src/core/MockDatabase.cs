using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Generic;


namespace PublicSafety
{
    public class MockDatabase
    {
        private BlobServiceClient bsc;

        public MockDatabase()
        {
            bsc = new BlobServiceClient(CredentialsProvider.AzureBlobStorageConnectionString());
        }


        #region "Public Safety Alerts"
        
        public async Task UploadPublicSafetyAlertsAsync(PublicSafetyAlert[] alerts)
        {
            BlobContainerClient bcc = bsc.GetBlobContainerClient("database");
            bcc.CreateIfNotExists();
            BlobClient bc = bcc.GetBlobClient("publicsafetyalerts");
            bc.DeleteIfExists();
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(alerts));
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            await bc.UploadAsync(ms);
        }

        public async Task<PublicSafetyAlert[]> DownloadPublicSafetyAlertsAsync()
        {
            BlobContainerClient bcc = bsc.GetBlobContainerClient("database");
            bcc.CreateIfNotExists();
            BlobClient bc = bcc.GetBlobClient("publicsafetyalerts");
            if (bc.Exists() == false)
            {
                return new PublicSafetyAlert[]{};
            }
            MemoryStream ms = new MemoryStream();
            bc.DownloadTo(ms);
            StreamReader sr = new StreamReader(ms);
            ms.Seek(0, SeekOrigin.Begin);
            string txt = await sr.ReadToEndAsync();
            PublicSafetyAlert[]? ToReturn = JsonConvert.DeserializeObject<PublicSafetyAlert[]>(txt);
            if (ToReturn != null)
            {
                return ToReturn;
            }
            else
            {
                throw new Exception("Unable to deserialize content (should be JSON) of blob.");
            }
        }

        public async Task AddPublicSafetyAlertAsync(PublicSafetyAlert psa)
        {
            PublicSafetyAlert[] psas = await DownloadPublicSafetyAlertsAsync();
            List<PublicSafetyAlert> ToAddTo = new List<PublicSafetyAlert>();
            foreach (PublicSafetyAlert opsa in psas)
            {
                ToAddTo.Add(opsa);
            }
            ToAddTo.Add(psa);
            await UploadPublicSafetyAlertsAsync(ToAddTo.ToArray());
        }

        public async Task ClearPublicSafetyAlertsAsync()
        {
            await UploadPublicSafetyAlertsAsync(new PublicSafetyAlert[]{});
        }

        #endregion

        #region "Webhook subscribers"

        public async Task UploadWebhookSubscriptionsAsync(WebhookSubscription[] subscriptions)
        {
            BlobContainerClient bcc = bsc.GetBlobContainerClient("database");
            bcc.CreateIfNotExists();
            BlobClient bc = bcc.GetBlobClient("webhooksubscriptions");
            bc.DeleteIfExists();
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(subscriptions));
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            await bc.UploadAsync(ms);
        }

        public async Task<WebhookSubscription[]> DownloadWebhookSubscriptionsAsync()
        {
            BlobContainerClient bcc = bsc.GetBlobContainerClient("database");
            bcc.CreateIfNotExists();
            BlobClient bc = bcc.GetBlobClient("webhooksubscriptions");
            if (bc.Exists() == false)
            {
                return new WebhookSubscription[]{};
            }
            MemoryStream ms = new MemoryStream();
            bc.DownloadTo(ms);
            StreamReader sr = new StreamReader(ms);
            ms.Seek(0, SeekOrigin.Begin);
            string txt = await sr.ReadToEndAsync();
            WebhookSubscription[]? ToReturn = JsonConvert.DeserializeObject<WebhookSubscription[]>(txt);
            if (ToReturn != null)
            {
                return ToReturn;
            }
            else
            {
                throw new Exception("Unable to deserialize content (should be JSON) of blob.");
            }
        }

        public async Task AddWebhookSubscriptionAsync(WebhookSubscription ws)
        {
            WebhookSubscription[] subs = await DownloadWebhookSubscriptionsAsync();
            List<WebhookSubscription> ToAddTo = new List<WebhookSubscription>();
            foreach (WebhookSubscription sub in subs)
            {
                ToAddTo.Add(sub);
            }
            ToAddTo.Add(ws);
            await UploadWebhookSubscriptionsAsync(ToAddTo.ToArray());
        }

        public async Task RemoveWebhookSubscriptionAsync(string id)
        {
            WebhookSubscription[] subs = await DownloadWebhookSubscriptionsAsync();
            List<WebhookSubscription> Filtered = new List<WebhookSubscription>();
            foreach (WebhookSubscription sub in subs)
            {
                if (sub.Id.ToLower() != id.ToLower())
                {
                    Filtered.Add(sub);
                }
            }
            await UploadWebhookSubscriptionsAsync(Filtered.ToArray());
        }

        public async Task ClearWebhookSubscriptionsAsync()
        {
            await UploadWebhookSubscriptionsAsync(new WebhookSubscription[]{});
        }

        #endregion

    }
}