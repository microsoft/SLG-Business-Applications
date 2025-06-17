using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using VehicleInspectionAI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace EVVIE_API
{
    [ApiController]
    [Route("inspect")]
    public class Inspect : ControllerBase
    {
        [HttpPost]
        public async Task Post()
        {
            //Extract body
            StreamReader sr = new StreamReader(Request.Body);
            string body = await sr.ReadToEndAsync();
            if (body == "")
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Body was empty!");
                return;
            }

            //Parse as JSON
            JObject bodyjo;
            try
            {
                bodyjo = JObject.Parse(body);
            }
            catch
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Provided body was not valid JSON.");
                return;
            }

            //Grab the "image" property (it is a long base64 string)
            JProperty? image = bodyjo.Property("image");
            JProperty? images = bodyjo.Property("images");
            if (image == null && images == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Must specify property 'image' with base64, including data URI scheme, or 'images' if you are providing multiple images as base64, encoded as a JSON array of strings. Neither 'image' nor 'images' was not specified in your request.");
                return;
            }

            //Create a list of base64s to analyze
            List<string> base64s = new List<string>();
            if (image != null)
            {
                base64s.Add(image.Value.ToString());
            }
            if (images != null)
            {
                string[]? imagesStrings = JsonConvert.DeserializeObject<string[]>(images.Value.ToString());
                if (imagesStrings != null)
                {
                    foreach (string imageString in imagesStrings)
                    {
                        base64s.Add(imageString);
                    }
                }
            }

            //If there are no base64s to analyze, return bad request
            if (base64s.Count == 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("No base64s were provided for analysis!");
                return;
            }
            
            //Perform inspection
            VehicleInspection vi;
            try
            {
                vi = await VehicleInspectionAgent.InspectAsync(base64s.ToArray());
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("There was an error while inspecting the provided base64 data(s): " + ex.Message);
                return;
            }

            //Return!
            Response.StatusCode = 200;
            Response.Headers["Content-Type"] = "application/json";
            await Response.WriteAsync(vi.ToJson().ToString());
        }
    }
}