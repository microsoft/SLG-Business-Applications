using System;
using TimHanewich.Dataverse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace CopilotStudioAnalytics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DoThis().Wait();
        }

        public static async Task DoThis()
        {

            //Parse Dataverse Credentials from JSON in file.
            string dataverse_authenticator_json_path = @".\DataverseAuthenticator.json";
            string dataverse_authenticator_json = System.IO.File.ReadAllText(dataverse_authenticator_json_path);
            DataverseAuthenticator? auth = JsonConvert.DeserializeObject<DataverseAuthenticator>(dataverse_authenticator_json);
            if (auth == null)
            {
                throw new Exception("Unable to parse DataverseAuthenticator credentials from file '" + dataverse_authenticator_json_path + "'");
            }
            auth.ClientId = Guid.Parse("51f81489-12ee-4a9e-aaae-a2591f45987d"); //fill in default (standard) clientid
            Console.WriteLine("Dataverse credentials received!");
            Console.Write("Authenticating as '" + auth.Username + "' to environment '" + auth.Resource + "'... ");
            
            //Authenticate (call to Dataverse OAuth API and get access token that we can then use to make)
            await auth.GetAccessTokenAsync();      
            Console.WriteLine("Authenticated!");     

            //Retrieve transcripts
            DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);
            Console.Write("Retrieving Copilot Studio transcripts... ");
            JArray transcripts = await ds.ReadAsync("conversationtranscripts");
            Console.WriteLine(transcripts.Count.ToString("#,##0") + " transcripts retrieved!");

            //Retrieve bots
            Console.Write("Retrieving list of Copilot bots... ");
            JArray bots = await ds.ReadAsync("bots");
            Console.WriteLine(bots.Count.ToString("#,##0") + " bots found!");
            
            //Parse them!
            CopilotStudioSession[] sessions = CopilotStudioSession.Parse(transcripts, bots);

            Console.WriteLine(JsonConvert.SerializeObject(sessions, Formatting.Indented));


        }
    }
}