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
    public class Inspect
    {
        //Pass this function an image or images, in base 64, and it will return a VehicleInspection.
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
            JProperty images = bodyjo.Property("images");
            if (image == null && images == null)
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("Must specify property 'image' with base64, including data URI scheme, or 'images' if you are providing multiple images as base64, encoded as a JSON array of strings. Neither 'image' nor 'images' was not specified in your request.");
                return bresp;
            }

            //Create a list of base64s to analyze
            List<string> base64s = new List<string>();
            if (image != null)
            {
                base64s.Add(image.Value.ToString());
            }
            if (images != null)
            {
                string[] imagesStrings = JsonConvert.DeserializeObject<string[]>(images.Value.ToString());
                foreach (string imageString in imagesStrings)
                {
                    base64s.Add(imageString);
                }
            }

            //If there are no base64s to analyze, return bad request
            if (base64s.Count == 0)
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("No base64s were provided for analysis!");
                return bresp;
            }
            
            //Perform inspection
            VehicleInspection vi;
            try
            {
                vi = await VehicleInspectionGPT.InspectAsync(base64s.ToArray());
            }
            catch (Exception ex)
            {
                HttpResponseMessage bresp = new HttpResponseMessage();
                bresp.StatusCode = HttpStatusCode.BadRequest;
                bresp.Content = new StringContent("There was an error while inspecting the provided base64 data(s): " + ex.Message);
                return bresp;
            }

            //Return!
            HttpResponseMessage ToReturn = new HttpResponseMessage();
            ToReturn.StatusCode = HttpStatusCode.OK;
            ToReturn.Content = new StringContent(vi.ToJson().ToString(), System.Text.Encoding.UTF8, "application/json");
            return ToReturn;
        }
    }
}