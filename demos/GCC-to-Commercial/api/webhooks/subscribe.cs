using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CRUX
{
    public class subscribe
    {
        [Function("subscribe")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            //Read the body
            StreamReader sr = new StreamReader(req.Body);
            string content = await sr.ReadToEndAsync();
            JObject jo = JObject.Parse(content);

            //Get the url property
            JProperty? prop_url = jo.Property("url");
            if (prop_url != null)
            {
                string url = prop_url.Value.ToString();
                EventSubscriberList esl = new EventSubscriberList(ConnectionStringProvider.GetConnectionString());
                await esl.AddAsync(url);

                HttpResponseData resp = req.CreateResponse();
                resp.Headers.Add("Location", "https://crux.azurewebsites.net/unsubscribe?url=" + url);
                resp.StatusCode = HttpStatusCode.Created;
                return resp;
            }
            else
            {
                HttpResponseData br = req.CreateResponse();
                br.StatusCode = HttpStatusCode.BadRequest;
                br.WriteString("Required property 'url' not present in body!");
                return br;
            }
        }
    }
}