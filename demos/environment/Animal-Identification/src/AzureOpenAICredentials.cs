using System;

namespace AnimalID
{
    public class AzureOpenAICredentials
    {
        public string Endpoint { get; set; }
        public string ApiKey { get; set; }

        public AzureOpenAICredentials()
        {
            Endpoint = "<Azure OpenAI Chat endpoint>";
            ApiKey = "<Azure OpenAI API key>";
        }
    }
}