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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CRUX
{
    public class unsubscribe
    {
        [Function("unsubscribe")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequestData req)
        {
            string queryDecoded = System.Web.HttpUtility.UrlDecode(req.Url.Query);
            NameValueCollection nvc = HttpUtility.ParseQueryString(queryDecoded);
            string? url = nvc.Get("url");
            if (url != null)
            {
                EventSubscriberList esl = new EventSubscriberList(ConnectionStringProvider.GetConnectionString());
                await esl.RemoveAsync(url);

                //Return
                HttpResponseData s = req.CreateResponse();
                s.StatusCode = HttpStatusCode.OK;
                return s;
            }
            else
            {
                HttpResponseData br = req.CreateResponse();
                br.StatusCode = HttpStatusCode.BadRequest;
                br.WriteString("Query parameter 'url' not specified!");
                return br;
            }
        }
    }
}