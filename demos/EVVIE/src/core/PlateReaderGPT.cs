using System;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace VehicleInspectionAI
{
    public class PlateReaderGPT
    {
        public static HttpRequestMessage PrepareRequest(string base64)
        {
            //Check if the base 64 they provide has the prefix in it (i.e. "data:image/png;base64,")
            if (base64.ToLower().Contains("data:image") == false || base64.ToLower().Contains("base64,") == false)
            {
                throw new Exception("When providing your base64 data, you must also include the data URI scheme before the base64 data itself. For example, the base64 string you provide should look like this: 'data:image/png;base64,iVBORw0KGgoAAAANSUhE...'.");
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
            SystemMessage.Add("content", "You are a helpful AI that assists in reading license plates.");
            messages.Add(SystemMessage);
            

            //Create URL portion of image content
            JObject UrlPortion = new JObject();
            UrlPortion.Add("url", base64);

            //Construct image url portion of user message
            JObject ImageUrlPortion = new JObject();
            ImageUrlPortion.Add("type", "image_url");
            ImageUrlPortion.Add("image_url", UrlPortion);

            //Create and add the image to the content
            JArray content = new JArray(); //multiple portions of the user's message (text & images)
            content.Add(ImageUrlPortion);

            //Now that all images have been added to the user's message, add the text prompt.
            JObject TextPortion = new JObject();
            TextPortion.Add("type", "text");
            TextPortion.Add("text", "What is the license plate number on this car? Respond in JSON in the following format: {\"plate\": \"<plate here>\"}. If there is no license plate visible in the image, simple set the \"plate\" property to \"N/A\".");
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
    
        public static async Task<string> ReadPlateAsync(string base64)
        {
            //Request
            HttpRequestMessage request = PrepareRequest(base64);
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

            //Strip out plate
            JObject contentjo = JObject.Parse(contentjson);
            JProperty? plate = contentjo.Property("plate");
            if (plate == null)
            {
                throw new Exception("Unable to find property 'plate' in the response from Azure OpenAI. It seems the model did not follow the correct format.");
            }
            string ToReturn = plate.Value.ToString();
            return ToReturn;
        }
    }
}