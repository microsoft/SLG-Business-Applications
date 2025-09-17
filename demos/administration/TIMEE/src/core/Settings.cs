using System;
using TimHanewich.AgentFramework;

namespace TIMEECore
{
    public class Settings
    {
        public static IModelConnection GetModelConnection()
        {
            AzureOpenAICredentials ToReturn = new AzureOpenAICredentials();
            ToReturn.URL = "(your Azure AI Foundry model endpoint here)";
            ToReturn.ApiKey = "(your Azure AI Foundry API Key here)";
            return ToReturn;
        }
    }
}