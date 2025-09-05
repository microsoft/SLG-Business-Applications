using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace AnimalID
{
    public class IdentificationOutput
    {
        public DateTime timestamp { get; set; }
        public AnimalID[] animals { get; set; }

        public IdentificationOutput()
        {
            animals = new AnimalID[] { };
        }

        private string Prompt
        {
            get
            {
                string ToReturn = @"Your job is to review photos captured by a wildlife game camera and identify any animals that may be spotted in the photos.

You will review a single photograph and then respond with your identification output in JSON in the following format. Below is an example:

{
    ""timestamp"": ""12/1/2025 9:45 AM"",
    ""animals"":
    [
        {
            ""species"": ""deer"",
            ""age"": ""adult"",
            ""sex"": ""unknown""
        },
        {
            ""species"": ""deer"",
            ""age"": ""juvinile"",
            ""sex"": ""male""
        }
    ]
}

As seen above, you will provide the timestamp as well. At the bottom of the image, you will see a banner that contains the timestamp.

You will then provide a list of every animal you see in the ""animals"" array, with a separate object for each individual animal you see. So, if you see 2 deer for example, you would return an array with two items in it.

For each animal, you will include the species, age, and sex. Age can be either ""juvenile"" or ""adult"" If you can't tell the age, assume adult. Sex will be either ""male"", ""female"", or ""unknown"". Use unknown if you cannot tel the sex.
                
If there are now visible animals in the photos, just return an empty animals array (i.e. ""[]"")                
                ";

                return ToReturn;
            }
        }

        public static string ImageToBase64(string path)
        {
            if (System.IO.File.Exists(path) == false)
            {
                throw new Exception("Image '" + path + "' does not exist.");
            }
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            string b64 = Convert.ToBase64String(bytes);
            return b64;
        }

        public static async Task<IdentificationOutput> IdentifyAsync(string image_path)
        {
            IdentificationOutput ToReturn = new IdentificationOutput();

            string b64 = ImageToBase64(image_path);
            b64 = "data:image/jpeg;base64," + b64;

            //Construct the body we will send
            JObject body = new JObject();
            JArray messages = new JArray();
            body.Add("messages", messages);

            //Add system prompt
            JObject SystemPrompt = new JObject();
            SystemPrompt.Add("role", "system");
            SystemPrompt.Add("content", ToReturn.Prompt);
            messages.Add(SystemPrompt);

            //Add user prompt (with image)
            JObject UserPrompt = new JObject();
            UserPrompt.Add("role", "user");
            JArray UserPromptContent = new JArray();
            UserPrompt.Add("content", UserPromptContent);
            messages.Add(UserPrompt);

            //Add user prompt instruction
            JObject UserPromptInstruction = new JObject();
            UserPromptInstruction.Add("type", "text");
            UserPromptInstruction.Add("text", "Identify animals in this picture");
            UserPromptContent.Add(UserPromptInstruction);

            //Add user prompt image
            JObject UserPromptImage = new JObject();
            UserPromptImage.Add("type", "image_url");
            JObject url = new JObject();
            url.Add("url", b64);
            UserPromptImage.Add("image_url", url);
            UserPromptContent.Add(UserPromptImage);

            //Make HTTP Post to Azure AI Service
            AzureOpenAICredentials creds = new AzureOpenAICredentials();
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Post;
            req.RequestUri = new Uri(creds.Endpoint);
            req.Headers.Add("api-key", creds.ApiKey);
            req.Content = new StringContent(body.ToString(), System.Text.Encoding.UTF8, "application/json");

            //Make call
            HttpClient hc = new HttpClient();
            HttpResponseMessage resp = await hc.SendAsync(req);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Call to Azure AI Service returned " + resp.StatusCode.ToString() + ": " + content);
            }


            return ToReturn;

        }


    }
}