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
    [Route("plate")]
    public class Plate : ControllerBase
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
            if (image == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Must specify property 'image' with base64, including data URI scheme. Property 'image' was not specified in your request.");
                return;
            }

            //Read the plate
            string plate = await PlateReaderAgent.ReadPlateAsync(image.Value.ToString());

            //Prepare response body
            JObject response = new JObject();
            response.Add("plate", plate);

            //Return!
            Response.StatusCode = 200;
            Response.Headers["Content-Type"] = "application/json";
            await Response.WriteAsync(response.ToString());
        }
    }
}