using System;
using TimHanewich.Dataverse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using Spectre.Console;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Nodes;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Text;
using TimHanewich.Dataverse.Metadata;

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
            //Create empty strings for secrets
            string username = "";
            string password = "";
            string resource = "";

            //Try to get these from the "key.json" file
            string expected_path = Path.Combine(Directory.GetCurrentDirectory(), "keys.json");
            if (System.IO.File.Exists(expected_path))
            {
                string keystxt = System.IO.File.ReadAllText(expected_path);
                JObject jokeys = JObject.Parse(keystxt);
                
                //Username?
                JProperty? prop_username = jokeys.Property("username");
                if (prop_username != null)
                {
                    username = prop_username.Value.ToString();
                }

                //Password?
                JProperty? prop_password = jokeys.Property("password");
                if (prop_password != null)
                {
                    password = prop_password.Value.ToString();
                }

                //Resource?
                JProperty? prop_resource = jokeys.Property("environment");
                if (prop_resource != null)
                {
                    resource = prop_resource.Value.ToString();
                }
            }

            //Get username if not already
            while (username == "")
            {
                Console.Write("Username to log into Dataverse > ");
                string? s = Console.ReadLine();
                if (s != null)
                {
                    username = s;
                }
            }

            //Get password if not already
            while (password == "")
            {
                Console.Write("Password to log into Dataverse > ");
                string? s = Console.ReadLine();
                if (s != null)
                {
                    password = s;
                }
            }

            //Get resource if not already
            while (resource == "")
            {
                Console.Write("Dataverse Environment URL > ");
                string? s = Console.ReadLine();
                if (s != null)
                {
                    resource = s;
                }
            }

            //Parse Dataverse Credentials from JSON in file.
            DataverseAuthenticator auth = new DataverseAuthenticator();
            auth.Username = username;
            auth.Password = password;
            auth.Resource = resource;
            auth.ClientId = Guid.Parse("51f81489-12ee-4a9e-aaae-a2591f45987d"); //fill in default (standard) clientid
            AnsiConsole.MarkupLine("[italic]Dataverse credentials locked and loaded![/]");

            //Infinite loop
            while (true)
            {
                //Authenticate (call to Dataverse OAuth API and get access token that we can then use to make)
                AnsiConsole.Markup("[green][bold]Dataverse Auth[/][/]: Authenticating as '" + auth.Username + "' to environment '" + auth.Resource + "'... ");
                await auth.GetAccessTokenAsync();    
                AnsiConsole.MarkupLine("[italic][green]Authenticated![/][/]");    

                DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);

                //Retrieve transcripts
                AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving Copilot Studio transcripts... ");
                JArray transcripts = await ds.ReadAsync("conversationtranscripts");
                AnsiConsole.MarkupLine("[italic][green]" + transcripts.Count.ToString("#,##0") + " transcripts retrieved![/][/]");

                //Retrieve bots
                AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving list of Copilot Studio bots... ");
                JArray bots = await ds.ReadAsync("bots");
                AnsiConsole.MarkupLine("[italic][green]" + bots.Count.ToString("#,##0") + " bots found![/][/]");

                //Parse them!
                CopilotStudioBot[] csbots = CopilotStudioBot.Parse(bots, transcripts);

                //Loop through all sessions, see if they've already been saved
                int SessionsUploaded = 0;
                int MessagesUploaded = 0;
                int SessionsSkipped = 0; //skipped due to it already being there
                foreach (CopilotStudioBot bot in csbots)
                {
                    foreach (CopilotStudioSession session in bot.Sessions)
                    {

                        //First, see if this session has already been saved
                        bool AlreadyDecompressedSession = false; //assume false
                        AnsiConsole.Markup("Checking if we alredy decompressed session '" + session.SessionId.ToString() + "'... ");
                        JArray records = await ds.ReadAsync("tsp_copilotsessions");
                        foreach (JObject tsp_copilotsession in records)
                        {
                            JProperty? id = tsp_copilotsession.Property("tsp_copilotsessionid");
                            if (id != null)
                            {
                                Guid idguid = Guid.Parse(id.Value.ToString());
                                if (idguid == session.SessionId)
                                {
                                    AlreadyDecompressedSession = true;
                                }
                            }
                        }

                        //Handle if we have or not already
                        if (!AlreadyDecompressedSession)
                        {
                            AnsiConsole.MarkupLine("[green]Not yet![/]");

                            //Upload the session
                            AnsiConsole.Markup("\tUploading session '" + session.SessionId.ToString() + "'... ");
                            await ds.CreateAsync("tsp_copilotsessions", session.ForDataverseUpload(bot));
                            SessionsUploaded = SessionsUploaded + 1;
                            AnsiConsole.MarkupLine("\t[green]Uploaded![/]");

                            //Upload each message
                            for (int i = 0; i < session.Messages.Length; i++)
                            {
                                AnsiConsole.Markup("\t" + "[gray]Uploading message " + (i + 1).ToString("#,##0") + " / " + session.Messages.Length.ToString("#,##0") + "...[/] ");
                                await ds.CreateAsync("tsp_copilotmessages", session.Messages[i].ForDataverseUpload(session.SessionId));
                                MessagesUploaded = MessagesUploaded + 1;
                                AnsiConsole.MarkupLine("[green]Uploadeded![/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[italic]already decompressed[/]");
                            SessionsSkipped = SessionsSkipped + 1;
                        }
                    }
                }

                //Print the results
                Console.WriteLine();
                AnsiConsole.MarkupLine("[green]Decompression complete![/]");
                AnsiConsole.MarkupLine("[bold]" + SessionsUploaded.ToString("#,##0") +  "[/] sessions uploaded");
                AnsiConsole.MarkupLine("[bold]" + MessagesUploaded.ToString("#,##0") +  "[/] messages uploaded");
                AnsiConsole.MarkupLine("[bold]" + SessionsSkipped.ToString("#,##0") + "[/] sessions skipped (already uploaded)");
                Console.WriteLine();

                //Wait 24 hours (do this once every 24 hours)
                DateTime NextLoopAt = DateTime.UtcNow.AddHours(24);
                while (DateTime.UtcNow < NextLoopAt)
                {
                    TimeSpan TimeUntilNextLoop = NextLoopAt - DateTime.UtcNow;

                    //Print time
                    string TimeToGo = TimeUntilNextLoop.Hours.ToString("#,##0") + " hours, " + TimeUntilNextLoop.Minutes.ToString("#,##0") + " minutes, " + TimeUntilNextLoop.Seconds.ToString("#,##0") + " seconds";
                    AnsiConsole.Markup("\r" + "[bold][blue]" + TimeToGo + "[/][/] until next update...");
                    await Task.Delay(1000); //wait 1 second
                }
                AnsiConsole.MarkupLine("[italic]Moving on to next loop now![/]");



            } //Infinite while loop ends here
        }
    }
}