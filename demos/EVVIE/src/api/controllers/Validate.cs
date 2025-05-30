using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using VehicleInspectionAI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EVVIE_API
{
    [ApiController]
    [Route("validate")]
    public class Validate : ControllerBase
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
            JProperty? prop_images = jo.Property("images");
            if (prop_images == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide your images as base64 in the 'images' property.");
                return;
            }

            //Parse the images value
            string[]? images = null;
            try
            {
                string prop_images_str = prop_images.Value.ToString();
                images = JsonConvert.DeserializeObject<string[]>(prop_images_str);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("The value you provided for the 'images' property did not parse into an string array. Msg: " + ex.Message);
                return;
            }

            //If images are null,
            if (images == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("The value you provided for the 'images' property did not parse into an string array.");
                return;
            }

            //Run it!
            ImageQualityIssue[] issues;
            try
            {
                issues = await ImageQualityValidationAgent.ValidateAsync(images);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("Image quality validation failed! Message: " + ex.Message);
                return;
            }

            //Respond
            Response.StatusCode = 200;
            Response.Headers["Content-Type"] = "application/json";
            JObject TR = new JObject();
            TR.Add("issues", JArray.Parse(JsonConvert.SerializeObject(issues)));
            await Response.WriteAsync(TR.ToString());
        }
    }
}