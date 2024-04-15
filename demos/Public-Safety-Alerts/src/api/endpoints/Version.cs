using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PublicSafety;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace PublicSafetyAPI
{
    public class Version
    {
        [Function("version")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            HttpResponseData resp  = req.CreateResponse();
            resp.StatusCode = System.Net.HttpStatusCode.OK;
            await resp.WriteStringAsync("0.2.0");
            return resp;
        }
    }
}