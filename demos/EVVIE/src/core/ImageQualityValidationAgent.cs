using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Encodings;
using System.Net;
using System.Net.Http;

namespace VehicleInspectionAI
{
    public class ImageQualityValidationAgent
    {
        public static string SystemPrompt
        {
            get
            {
                return @"You are Image Quality Validation Agent. You will be provided images of police vehicles that have damage to them. These images are being collected and later assessed with the purpose of noting any damage that may have occured during the line of work. Your role in this process is to ensure the images are sufficient in their ability to reflect damage. Your role is to validate that and if there is a problem, let us know.

Specifically, your job is to ensure the images meet the following requirements:
- The person taking the image is not TOO FAR away from the vehicle where the damage is not visible. If they are more than 15 feet away from the vehicle, that is not acceptable.
- The image has sufficient lighting to show the damage. If the picture is being taken in a very dark environment where the damage would be difficult to detect, that is not acceptable.
- The image is clear, in focus, and not blurry. If the image appears to just be blurry or very out of focus (the image was not taken correctly), that is not acceptable.

If there is an issue with and of the images, as described above, you are to document these problems as a JSON array as property ""issues"" in a JSON object, like below for example:
{
    ""issues"":
    [
        {
            ""title"": ""Seek Better Lighting"",
            ""description"": ""The image is somewhat dark and it is difficult to tell if there is damage. Please re-take these images in an environment with brighter lighting or use a flashlight of some sort.""
        },
        {
            ""title"": ""Stand Closer"",
            ""description"": ""You appear to be far away from the vehicle! Please stand closer, within 15 feet, to get a better photo.""
        }
    ]
}


As you can see above, the messages you log will be shown directly to the officer that is taking the images, so please be polite and constructive. Log as many as you need, or none at all. Similar to the examples above, always log both a title for the issue but then a further description that instructs the officer on how to fix the issue.

If there is no problem with the images and they accurately depict damage (or lack of damage) well, leave it alone! No need to report any problems, so just simply return an empty JSON array for the ""issues"" property, like this for example:

{""issues"":[]}

                ";
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
            TextPortion.Add("text", "These are pictures of damage to a police vehicle. Validate these images are of high enough quality to note any damage (or lack of damage).");
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
            ToReturn.Content = new StringContent(body.ToString(), System.Text.Encoding.UTF8, "application/json");

            return ToReturn;
        }

        public static async Task<string[]> ValidateAsync(params string[] image_base64s)
        {
            HttpRequestMessage request = PrepareRequest(image_base64s);
            HttpClient hc = new HttpClient();
            HttpResponseMessage response = await hc.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Request to Azure OpenAI services returned status code '" + response.StatusCode.ToString() + "'! Msg: " + content);
            }

            //Extract the "content" property from the entire response (what the models response was)
            JObject ResponseBody = JObject.Parse(content);
            JToken? rcontent = ResponseBody.SelectToken("choices[0].message.content");
            if (rcontent == null)
            {
                throw new Exception("Unable to find content property from Azure OpenAI response!");
            }
            string contentjson = rcontent.ToString();

            //Now that we have the content that should be JSON (the models response), get the 'issues' property out of it, which should be a string array
            JObject robject;
            try
            {
                robject = JObject.Parse(contentjson);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an issue while parsing the model's response as JSON. It didn't seem to follow a structured JSON format. Internal error: " + ex.Message);
            }

            //Get issues
            JProperty? issues = robject.Property("issues");
            if (issues == null)
            {
                throw new Exception("Property 'issues' was not in the response JSON payload! The model did not document the issues correctly.");
            }
            string issues_str = issues.Value.ToString();

            //Unpack issues as JSON array
            string[]? ToReturn;
            try
            {
                ToReturn = JsonConvert.DeserializeObject<string[]>(issues_str);
            }
            catch (Exception ex)
            {
                throw new Exception("The 'issues' response from the model was not an array of strings! Msg: " + ex.Message);
            }

            //if empty
            if (ToReturn == null)
            {
                throw new Exception("For some reason, the issues property did not successfully parse into an array of strings.");
            }


            return ToReturn;
        }

    }
}