using System;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace VehicleInspectionAI
{
    public class VehicleInspectionGPT
    {

        public static string SystemPrompt
        {
            get
            {
                return @"You will be provided pictures of damage to police vehicles, assets of the LA County Sheriff's department. The vehicles are being inspected as part of a regular before-shift check. Your job is to classify this damage into the following format:

{
    ""area"": ""<area of the car the damage is to here>"",
    ""description"": ""<describe the damage to the car>"",
    ""severityLevel"": 3
}

The ""area"" property can be one of the following areas below:
- doors
- hood
- trunk
- front bumper
- rear bumper
- windshield
- door windows
- tires
- wheels
- headlights

Severity level is an integer, a rating of how severe the damage to th car is, from 1 (least severe) to 5 (most severe), described below.
- 1 = minor cosmetic damage, like a small scuff in the paint, tiny chips in the windshield, etc.
- 2 = light damage, like minor dents and scratches that are significant but do not affect the vehicle's function.
- 3 = Moderate damage, like large dents, cracked components.
- 4 = Major damage to the car that possibly inhibits its ability to be used in law enforcement.
- 5 = Crticial damage that affects the vehicles integrity and safety.

If there is no visible damage to the vehicle, set area to ""NoDamage"", set severityLevel to 0, and put ""No visible damage to the vehicle"" in the description.";
            }
        }

        //Prepares the HTTP request message to Azure OpenAI services
        public static HttpRequestMessage PrepareRequest(params string[] base64s)
        {
            //Check if the base 64 they provide has the prefix in it (i.e. "data:image/png;base64,")
            foreach (string base64 in base64s)
            {
                if (base64.ToLower().Contains("data:image") == false || base64.ToLower().Contains("base64,") == false)
                {
                    throw new Exception("When providing your base64 data, you must also include the data URI scheme before the base64 data itself. For example, the base64 string you provide should look like this: 'data:image/png;base64,iVBORw0KGgoAAAANSUhE...'.");
                }
            }
            
            HttpRequestMessage ToReturn = new HttpRequestMessage();
            ToReturn.Method = HttpMethod.Post;
            ToReturn.RequestUri = new Uri(AzureOpenAICredentialsProvider.RequestUrl);
            ToReturn.Headers.Add("api-key", AzureOpenAICredentialsProvider.ApiKey);

            //Construct list of messages
            JArray messages = new JArray();

            //Construct system message and add it
            JObject SystemMessage = new JObject();
            SystemMessage.Add("role", "system");
            SystemMessage.Add("content", SystemPrompt);
            messages.Add(SystemMessage);


            //Now we will start working on the user message
            JArray content = new JArray(); //multiple portions of the user's message (text & images)
            foreach (string base64 in base64s)
            {
                //Construct url portion
                JObject UrlPortion = new JObject();
                UrlPortion.Add("url", base64);

                //Construct image url portion of user message
                JObject ImageUrlPortion = new JObject();
                ImageUrlPortion.Add("type", "image_url");
                ImageUrlPortion.Add("image_url", UrlPortion);

                //Add to the content
                content.Add(ImageUrlPortion);
            }

            //Now that all images have been added to the user's message, add the text prompt.
            JObject TextPortion = new JObject();
            TextPortion.Add("type", "text");
            TextPortion.Add("text", "This is a picture of damage to a Police Officer's vehicle. Document the damage to the vehicle, if any, in JSON.");
            content.Add(TextPortion);
            
            //Construct user message and add it
            JObject UserMessage = new JObject();
            UserMessage.Add("role", "user");
            UserMessage.Add("content", content);
            messages.Add(UserMessage);

            //Start to consturct the body
            JObject body = new JObject();
            body.Add("messages", messages);

            //Add JSON mode
            JObject JsonObjectType = new JObject();
            JsonObjectType.Add("type", "json_object");
            body.Add("response_format", JsonObjectType);

            //Add the body
            ToReturn.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");

            return ToReturn;
        }
    
        //Calls to Azure OpenAI services, parsing out the JSON as a JObject
        public static async Task<JObject> CallAsync(params string[] base64s)
        {
            HttpRequestMessage request = PrepareRequest(base64s);
            HttpClient hc = new HttpClient();
            HttpResponseMessage response = await hc.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Request to Azure OpenAI services returned status code '" + response.StatusCode.ToString() + "'! Msg: " + content);
            }
            JObject ResponseBody = JObject.Parse(content);
            JToken? rcontent = ResponseBody.SelectToken("choices[0].message.content");
            if (rcontent == null)
            {
                throw new Exception("Unable to find content property from Azure OpenAI response!");
            }
            string contentjson = rcontent.ToString();

            JObject ToReturn = JObject.Parse(contentjson);
            return ToReturn;
        }

        public static async Task<VehicleInspection> InspectAsync(params string[] base64s)
        {
            JObject jo = await CallAsync(base64s);

            VehicleInspection ToReturn = new VehicleInspection();

            //area
            JProperty? prop_area = jo.Property("area");
            if (prop_area != null)
            {
                string area = prop_area.Value.ToString();
                if (area == "NoDamage")
                {
                    ToReturn.Area = CarArea.NoDamage;
                }
                else if (area == "doors")
                {
                    ToReturn.Area = CarArea.Doors;
                }
                else if (area == "hood")
                {
                    ToReturn.Area = CarArea.Hood;
                }
                else if (area == "trunk")
                {
                    ToReturn.Area = CarArea.Trunk;
                }
                else if (area == "front bumper")
                {
                    ToReturn.Area = CarArea.FrontBumper;
                }
                else if (area == "rear bumper")
                {
                    ToReturn.Area = CarArea.RearBumper;
                }
                else if (area == "windshield")
                {
                    ToReturn.Area = CarArea.Windshield;
                }
                else if (area == "door windows")
                {
                    ToReturn.Area = CarArea.DoorWindows;
                }
                else if (area == "tires")
                {
                    ToReturn.Area = CarArea.Tires;
                }
                else if (area == "wheels")
                {
                    ToReturn.Area = CarArea.Wheels;
                }
                else if (area == "headlights")
                {
                    ToReturn.Area = CarArea.Headlights;
                }
                else
                {
                    ToReturn.Area = CarArea.Unknown;
                }
            }

            //Description
            JProperty? prop_description = jo.Property("description");
            if (prop_description != null)
            {
                ToReturn.Description = prop_description.Value.ToString();
            }

            //Severity level
            JProperty? prop_severityLevel = jo.Property("severityLevel");
            if (prop_severityLevel != null)
            {
                ToReturn.SeverityLevel = Convert.ToInt32(prop_severityLevel.Value.ToString());
            }

            return ToReturn;
        }
    }
}