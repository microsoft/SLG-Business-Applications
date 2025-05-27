using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace VehicleInspectionAI
{
    public class OdometerAgent
    {
        public static async Task<int> ReadOdometerAsync(string image_b64)
        { 
            //Check if the base 64 they provide has the prefix in it (i.e. "data:image/png;base64,")
            if (image_b64.ToLower().Contains("data:image") == false || image_b64.ToLower().Contains("base64,") == false)
            {
                throw new Exception("When providing your base64 data, you must also include the data URI scheme before the base64 data itself. For example, the base64 string you provide should look like this: 'data:image/png;base64,iVBORw0KGgoAAAANSUhE...'.");
            }

            HttpRequestMessage reqmsg = new HttpRequestMessage();
            reqmsg.Method = HttpMethod.Post;
            reqmsg.RequestUri = new Uri(AzureOpenAICredentialsProvider.RequestUrl);
            reqmsg.Headers.Add("api-key", AzureOpenAICredentialsProvider.ApiKey);

            //Construct list of messages
            JArray messages = new JArray();

            //Construct system message and add it
            string SystemPrompt = "Your job is to read images of odometers and report the reading (in miles). Always provide the reading as an integer in JSON format, like this: {\"odometer\": 123456}";
            JObject SystemMessage = new JObject();
            SystemMessage.Add("role", "system");
            SystemMessage.Add("content", SystemPrompt);
            messages.Add(SystemMessage);


            //Now we will start working on the user message
            JArray content = new JArray(); //multiple portions of the user's message (text & images)

            //Construct url portion
            JObject UrlPortion = new JObject();
            UrlPortion.Add("url", image_b64);

            //Construct image url portion of user message
            JObject ImageUrlPortion = new JObject();
            ImageUrlPortion.Add("type", "image_url");
            ImageUrlPortion.Add("image_url", UrlPortion);

            //Add to the content
            content.Add(ImageUrlPortion);

            //Now that all images have been added to the user's message, add the text prompt.
            JObject TextPortion = new JObject();
            TextPortion.Add("type", "text");
            TextPortion.Add("text", "What is the reading of this odometer?");
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
            reqmsg.Content = new StringContent(body.ToString(), System.Text.Encoding.UTF8, "application/json");

            //Make the HTTP Call
            HttpClient hc = new HttpClient();
            HttpResponseMessage response = await hc.SendAsync(reqmsg);
            string response_body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Request to Azure OpenAI services returned status code '" + response.StatusCode.ToString() + "'! Msg: " + content);
            }

            //Extract the "content" property from the entire response (what the models response was)
            JObject ResponseBody = JObject.Parse(response_body);
            JToken? rcontent = ResponseBody.SelectToken("choices[0].message.content");
            if (rcontent == null)
            {
                throw new Exception("Unable to find content property from Azure OpenAI response!");
            }
            string contentjson = rcontent.ToString();

            //Now that we have the content that should be JSON (the models response), parse it
            JObject robject;
            try
            {
                robject = JObject.Parse(contentjson);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an issue while parsing the model's response as JSON. It didn't seem to follow a structured JSON format. Internal error: " + ex.Message);
            }

            //Get odometer reading
            JProperty? odometer = robject.Property("odometer");
            if (odometer == null)
            {
                throw new Exception("Property 'odometer' was not in the response JSON payload! The model did not report the odometer reading as expected.");
            }
            string odo_str = odometer.Value.ToString();

            //Parse odometer into int
            int odo;
            try
            {
                odo = Convert.ToInt32(odo_str);
            }
            catch
            {
                throw new Exception("Returned odometer value from model '" + odo_str + "' was not convertible to a string.");
            }

            return odo;
        }
    }
}