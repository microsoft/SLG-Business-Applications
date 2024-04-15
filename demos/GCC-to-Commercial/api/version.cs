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
    public class version
    {
        [Function("version")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData ToReturn = req.CreateResponse();
            ToReturn.StatusCode = HttpStatusCode.OK;
            ToReturn.Headers.Add("Content-Type", "text/plain");
            ToReturn.WriteString("0.2.2");
            return ToReturn;
        }
    }
}