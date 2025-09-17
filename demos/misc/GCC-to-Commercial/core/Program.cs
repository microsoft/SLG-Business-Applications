using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRUX
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            EventSubscriberList esl = new EventSubscriberList(ConnectionStringProvider.GetConnectionString());
            esl.RemoveAsync("https://google.com").Wait();

            string[] subs = esl.RetrieveAsync().Result;
            Console.WriteLine(JsonConvert.SerializeObject(subs, Formatting.Indented));
        }
    }
}