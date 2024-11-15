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
using System.Collections.Generic;

namespace VehicleInspectionsAPI
{
    public class Plate
    {
        //Provide this function with a base64 of an image and get back a license plate in that image.
        [FunctionName("plate")]
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
                bresp.Content = new StringContent("Must specify property 'image' with base64, including data URI scheme. Property 'image' was not specified in your request.");
                return bresp;
            }

            //Read the plate
            string plate = await PlateReaderGPT.ReadPlateAsync(image.Value.ToString());

            //Prepare response body
            JObject response = new JObject();
            response.Add("plate", plate);

            //Return!
            HttpResponseMessage ToReturn = new HttpResponseMessage();
            ToReturn.StatusCode = HttpStatusCode.OK;
            ToReturn.Content = new StringContent(response.ToString(), System.Text.Encoding.UTF8, "application/json");
            return ToReturn;
        }
    }
}