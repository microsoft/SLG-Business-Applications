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
    public class EventBroker
    {

        private ILogger log;

        public EventBroker(ILoggerFactory factory)
        {
            log = factory.CreateLogger<EventBroker>();
        }

        [Function("eventbroker")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {

            //Read the body
            StreamReader sr = new StreamReader(req.Body);
            string content = await sr.ReadToEndAsync();
            JArray ja = JArray.Parse(content);

            //Is there a validation code? If there is, this is an attempt to validate this webhook subscription (the "handshake")
            JToken? validationUrl = ja[0].SelectToken("data.validationUrl");
            if (validationUrl != null)
            {
                log.LogInformation("It is a validation call!");
                string validation_url = validationUrl.ToString();
                log.LogInformation("Validation URL = <" + validation_url + ">"); 
                HttpRequestMessage vreq = new HttpRequestMessage();
                vreq.Method = HttpMethod.Get;
                vreq.RequestUri = new Uri(validation_url);
                HttpClient hc = new HttpClient();
                log.LogInformation("Making GET call to validation URL...");
                HttpResponseMessage subresp = await hc.SendAsync(vreq);
                string scontent = await subresp.Content.ReadAsStringAsync();
                log.LogInformation("Complete with response code '" + subresp.StatusCode.ToString() + "'! Response content: " + scontent);
            }
            else //so it is not a validation (handshake) - it is an alert of a blob being created/edited/whatever!
            {
                log.LogInformation("validationUrl was not found. It must be an event update!");
                JToken? eventType = ja[0].SelectToken("eventType");
                if (eventType != null)
                {
                    if (eventType.ToString() == "Microsoft.Storage.BlobCreated")
                    {

                        //Get the url
                        JToken? _url = ja[0].SelectToken("data.url");
                        if (_url != null)
                        {
                            //Get the content type
                            string contentType = "";
                            JToken? ct = ja[0].SelectToken("data.contentType");
                            if (ct != null)
                            {
                                contentType = ct.ToString();
                            }

                            //Get the content length (in bytes)
                            int contentLength = 0;
                            JToken? cl = ja[0].SelectToken("data.contentLength");
                            if (cl != null)
                            {
                                contentLength = Convert.ToInt32(cl);
                            }

                            string url = _url.ToString();
                            int loc1 = url.LastIndexOf("/");
                            string blob_name = url.Substring(loc1+1);
                            int loc2 = url.LastIndexOf("/", loc1 - 1);
                            string container_name = url.Substring(loc2 + 1, loc1 - loc2 - 1);


                            //Prepare what we will send to each subscriber
                            JObject jo = new JObject();
                            jo.Add("name", blob_name);
                            jo.Add("container", container_name);
                            jo.Add("url", url);
                            jo.Add("type", contentType);
                            jo.Add("size", contentLength);

                            //Get all subscribers and post to each
                            if (container_name != "settings" && container_name != "$logs" && container_name != "azure-webjobs-host")
                            {
                                EventSubscriberList esl = new EventSubscriberList(ConnectionStringProvider.GetConnectionString());
                                string[] subs = await esl.RetrieveAsync();
                                HttpClient hc = new HttpClient();
                                foreach (string sub in subs)
                                {
                                    HttpRequestMessage preq = new HttpRequestMessage();
                                    preq.RequestUri = new Uri(sub);
                                    preq.Method = HttpMethod.Post;
                                    preq.Content = new StringContent(jo.ToString(), System.Text.Encoding.UTF8, "application/json");
                                    await hc.SendAsync(preq);
                                }
                            }
                            else
                            {
                                log.LogInformation("The container name was a skip name (" + container_name + "). Not sending out events.");
                            }
                        }                       
                    }
                }
            }

            


            //Return 200 OK
            log.LogInformation("Returning 200 OK");
            HttpResponseData resp = req.CreateResponse();
            resp.StatusCode = HttpStatusCode.OK;
            return resp;
        }
    }
}