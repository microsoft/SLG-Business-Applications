using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CRUX
{
    public class EventSubscriberList
    {
        private BlobServiceClient bsc;

        public EventSubscriberList(string connection_string)
        {
            bsc = new BlobServiceClient(connection_string);
        }

        public async Task<string[]> RetrieveAsync()
        {
            BlobContainerClient bcc = bsc.GetBlobContainerClient("settings");
            if (bcc.Exists() == false)
            {
                bcc.Create();
            }
            BlobClient bc = bcc.GetBlobClient("subscribers.json");

            if (bc.Exists() == false)
            {
                return new string[]{};
            }
            
            MemoryStream ms = new MemoryStream();
            await bc.DownloadToAsync(ms);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string content = await sr.ReadToEndAsync();

            string[]? list = JsonConvert.DeserializeObject<string[]>(content);
            if (list != null)
            {
                return list;
            }
            else
            {
                throw new Exception("Unable to parse subscriber list from content.");
            }
        }

        private async Task OverwriteListAsync(string[] list)
        {
            BlobContainerClient bcc = bsc.GetBlobContainerClient("settings");
            if (bcc.Exists() == false)
            {
                bcc.Create();
            }
            BlobClient bc = bcc.GetBlobClient("subscribers.json");

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            await sw.WriteAsync(JsonConvert.SerializeObject(list));
            sw.Flush();
            ms.Position = 0;
            await bc.UploadAsync(ms, true);
        }

        public async Task AddAsync(string endpoint)
        {
            string[] current_list = await RetrieveAsync();
            List<string> NewList = new List<string>();
            NewList.AddRange(current_list);
            NewList.Add(endpoint);
            await OverwriteListAsync(NewList.ToArray());
        }

        public async Task RemoveAsync(string endpoint)
        {
            string[] current_list = await RetrieveAsync();
            List<string> NewList = new List<string>();
            NewList.AddRange(current_list);
            NewList.Remove(endpoint);
            await OverwriteListAsync(NewList.ToArray());
        }
    }
}