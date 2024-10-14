using System;
using TimHanewich.Dataverse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using Spectre.Console;

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
            AnsiConsole.MarkupLine("Welcome to :robot: [bold][blue]Copilot Studio Advanced Analytics[/][/] :robot:, a demonstration application by the [underline]Microsoft State & Local Government Business Applications team[/]!");
            

            //Parse Dataverse Credentials from JSON in file.
            string dataverse_authenticator_json_path = @".\DataverseAuthenticator.json";
            string dataverse_authenticator_json = System.IO.File.ReadAllText(dataverse_authenticator_json_path);
            DataverseAuthenticator? auth = JsonConvert.DeserializeObject<DataverseAuthenticator>(dataverse_authenticator_json);
            if (auth == null)
            {
                throw new Exception("Unable to parse DataverseAuthenticator credentials from file '" + dataverse_authenticator_json_path + "'");
            }
            auth.ClientId = Guid.Parse("51f81489-12ee-4a9e-aaae-a2591f45987d"); //fill in default (standard) clientid
            AnsiConsole.MarkupLine("[italic]Dataverse credentials received![/]");
            
            //Authenticate (call to Dataverse OAuth API and get access token that we can then use to make)
            AnsiConsole.Markup("[green][bold]Dataverse Auth[/][/]: Authenticating as '" + auth.Username + "' to environment '" + auth.Resource + "'... ");
            await auth.GetAccessTokenAsync();      
            AnsiConsole.MarkupLine("[italic][green]Authenticated![/][/]");     

            //Retrieve transcripts
            DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);
            AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving Copilot Studio transcripts... ");
            //JArray transcripts = await ds.ReadAsync("conversationtranscripts");
            JArray transcripts = JArray.Parse(System.IO.File.ReadAllText(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\Copilot-Studio-Advanced-Analytics\examples\conversationtranscripts.json")); //Collect from local, optionally
            AnsiConsole.MarkupLine("[italic][green]" + transcripts.Count.ToString("#,##0") + " transcripts retrieved![/][/]");

            //Retrieve bots
            AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving list of Copilot Studio bots... ");
            JArray bots = await ds.ReadAsync("bots");
            AnsiConsole.MarkupLine("[italic][green]" + bots.Count.ToString("#,##0") + " bots found![/][/]");
            
            //Parse them!
            CopilotStudioBot[] csbots = CopilotStudioBot.Parse(bots, transcripts);


            Tree root = new Tree("Bots in environment '" + auth.Resource + "'");
            
            //Add each bot
            foreach (CopilotStudioBot csbot in csbots)
            {
                TreeNode botnode = root.AddNode(csbot.Name + " (" + csbot.SchemaName + ")");

                //Add # of sessions
                TreeNode sessions = botnode.AddNode(csbot.Sessions.Length.ToString() + " sessions");

                //Add # of messages
                int msgs = 0;
                foreach (CopilotStudioSession ses in csbot.Sessions)
                {
                    msgs = msgs + ses.TurnCount;
                }
                TreeNode messages = botnode.AddNode(msgs.ToString("#,##0") + " exchanged messages");

                //Add # of citations
                int citations = 0;
                foreach (CopilotStudioSession ses in csbot.Sessions)
                {
                    citations = citations + ses.Citations.Length;
                }
                TreeNode ncitations = botnode.AddNode(citations.ToString("#,##0") + " citations");
            }


            AnsiConsole.Write(root);

            Console.ReadLine();







            //Present a list of options
            SelectionPrompt<string> BotSelection = new SelectionPrompt<string>();
            BotSelection.Title("Which bot would you like to learn more about?");
            foreach (CopilotStudioBot csbot in csbots)
            {
                BotSelection.AddChoice(csbot.PreviewText());
            }

            string SelectedBot = AnsiConsole.Prompt(BotSelection);
            Console.WriteLine("You selected '" + SelectedBot + "'");

            //Print
            //Console.WriteLine(JsonConvert.SerializeObject(csbots, Formatting.Indented));
        }
    }
}