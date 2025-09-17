using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using VehicleInspectionAI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EVVIE_API
{
    [ApiController]
    [Route("odometer")]
    public class Odometer : ControllerBase
    {
        [HttpPost]
        public async Task Post()
        {
            //Check JSON header
            string? contenttype = Request.Headers["Content-Type"];
            if (contenttype == null || contenttype.ToLower() != "application/json")
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide your data in JSON format and declare the application/json Content-Type header!");
                return;
            }

            //Get out body
            StreamReader sr = new StreamReader(Request.Body);
            string body = await sr.ReadToEndAsync();
            JObject jo;
            try
            {
                jo = JObject.Parse(body);
            }
            catch
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide your data as JSON!");
                return;
            }

            //Get the images property
            JProperty? prop_image = jo.Property("image");
            if (prop_image == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide your image as base64 in the 'image' property.");
                return;
            }

            //Get the image value!
            string image_b64 = prop_image.Value.ToString();

            //Perform the odometer reading
            int odo;
            try
            {
                odo = await OdometerReaderAgent.ReadOdometerAsync(image_b64);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("Reading of odometer failed! Message: " + ex.Message);
                return;
            }

            //Respond
            Response.StatusCode = 200;
            Response.Headers["Content-Type"] = "application/json";
            JObject TR = new JObject();
            TR.Add("reading", odo);
            await Response.WriteAsync(TR.ToString());
        }
    }
}