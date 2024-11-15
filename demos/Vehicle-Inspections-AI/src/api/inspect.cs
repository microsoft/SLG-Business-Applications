using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using VehicleInspectionAI;

namespace VehicleInspectionsAPI
{
    public class Inspect
    {
        [FunctionName("inspect")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            //Extract body
            StreamReader sr = new StreamReader(req.Body);
            string body = await sr.ReadToEndAsync();
            if (body == "")
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("Body was empty!");
                return bresp;
            }

            //Parse as JSON
            JObject bodyjo;
            try
            {
                bodyjo = JObject.Parse(body);
            }
            catch
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("Provided body was not valid JSON.");
                return bresp;
            }

            //Grab the "image" property (it is a long base64 string)
            JProperty image = bodyjo.Property("image");
            if (image == null)
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("Must specify property 'image' with base64, including data URI scheme. 'image' property was not specified in your request.");
                return bresp;
            }
            string imagestr = image.Value.ToString();

            //Perform inspection
            VehicleInspection vi;
            try
            {
                vi = await VehicleInspectionGPT.InspectAsync(imagestr);
            }
            catch (Exception ex)
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("There was an error while inspecting the provided base64 data: " + ex.Message);
                return bresp;
            }

            //Return!
            HttpResponseMessage ToReturn = new HttpResponseMessage();
            ToReturn.StatusCode = HttpStatusCode.OK;
            ToReturn.Content = new StringContent(JsonConvert.SerializeObject(vi), System.Text.Encoding.UTF8, "application/json");
            return ToReturn;
        }
    }
}