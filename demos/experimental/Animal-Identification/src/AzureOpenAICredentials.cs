using System;

namespace AnimalID
{
    public class AzureOpenAICredentials
    {
        public string Endpoint { get; set; }
        public string ApiKey { get; set; }

        public AzureOpenAICredentials()
        {
            Endpoint = "<your chat completions endpoint>";
            ApiKey = "<your api key>";
        }
    }
}