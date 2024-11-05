using System;
using TimHanewich.Dataverse;
using Spectre.Console;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimHanewich.Dataverse.Metadata;

namespace DataversePerformanceTesting
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            MainProgramAsync().Wait();
        }

        public static async Task MainProgramAsync()
        {
            //Create empty strings for secrets
            string username = "";
            string password = "";
            string resource = "";

            //Try to get these from the "key.json" file
            string expected_path = @".\keys.json";
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

            //Give option on what to do
            SelectionPrompt<string> ToPerformChoice = new SelectionPrompt<string>();
            ToPerformChoice.Title("What do you want to do?");
            ToPerformChoice.AddChoice("Test #1 - Upload one-by-one");
            string ToPerform = AnsiConsole.Prompt(ToPerformChoice);

            //Handle
            if (ToPerform == "Test #1 - Upload one-by-one")
            {
                
                //Authenticate against Dataverse
                AnsiConsole.Markup("Authenticating against Dataverse... ");
                await auth.GetAccessTokenAsync();
                Console.WriteLine(auth.AccessToken);
                AnsiConsole.MarkupLine("[green]Success![/]");
                TimeSpan GoodFor = auth.AccessTokenExpiresUtc - DateTime.UtcNow;
                AnsiConsole.MarkupLine("Access token is good for [bold]" + GoodFor.TotalMinutes.ToString("#,##0") + " minutes[/]");

                //Validate the Animal table exists
                DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);
                AnsiConsole.Markup("Downloading metadata... ");
                EntityMetadataSummary[] metadatasummaries = await ds.GetEntityMetadataSummariesAsync();
                AnsiConsole.MarkupLine(metadatasummaries.Length.ToString("#,##0") + " tables found.");
                bool AnimalTableExists = false;
                foreach (EntityMetadataSummary ems in metadatasummaries)
                {
                    if (ems.EntitySetName == "timh_animals")
                    {
                        AnimalTableExists = true;
                    }
                }
                if (AnimalTableExists)
                {
                    AnsiConsole.MarkupLine("[green]timh_animals table confirmed to exist in environment '" + auth.Resource + "'![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]timh_animals table does not exist in environment '" + auth.Resource + "'! Make sure you install the necessary solution in this environment before continuing.[/]");
                    Environment.Exit(0);
                }

                //Begin recording
                DateTime UploadStarted = DateTime.UtcNow;
                int RecordsUploaded = 0;
                bool EncounteredUploadError = false;
                while (EncounteredUploadError == false)
                {
                    TimeSpan Remaining = auth.AccessTokenExpiresUtc - DateTime.UtcNow; //Estimate time remaining until token expires
                    TimeSpan Elapsed = DateTime.UtcNow - UploadStarted;
                    float RecordsPerMinute = Convert.ToSingle(RecordsUploaded) / Convert.ToSingle(Elapsed.TotalMinutes); //The avg records per minute so far
                    int UploadEstimate = Convert.ToInt32(RecordsUploaded + (RecordsPerMinute * Convert.ToSingle(Remaining.TotalMinutes))); //An estimate for how many records will be uploaded during this entire test, based on the trailing performance.

                    Animal ToUpload = Animal.Random();
                    AnsiConsole.Markup("[gray](" + Remaining.TotalMinutes.ToString("#,##0") + " mins remaining, est. " + UploadEstimate.ToString("#,##0") + " records @ " + RecordsPerMinute.ToString("#,##0.0") + " records/min)[/] " + "Uploading animal #" + (RecordsUploaded + 1).ToString("#,##0") + " (" + ToUpload.Name + ")... ");
                    try
                    {
                        await ds.CreateAsync("timh_animals", ToUpload.ForDataverseUpload());
                        RecordsUploaded = RecordsUploaded + 1;
                        AnsiConsole.MarkupLine("[green]Uploaded![/]");
                    }
                    catch (Exception ex)
                    {
                        EncounteredUploadError = true;
                        AnsiConsole.MarkupLine("[red]Error! Msg: " + ex.Message + "[/]");
                    }
                }
                DateTime UploadEnded = DateTime.UtcNow;

                //Print statistics
                TimeSpan TotalUploadTime = UploadEnded - UploadStarted;
                AnsiConsole.MarkupLine("[bold]" + RecordsUploaded.ToString("#,##0") + "[/] records uploaded in [bold]" + TotalUploadTime.TotalSeconds.ToString("#,##0") + " seconds[/]!");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]I do not know how to handle selection '" + ToPerform + "'![/]");
            }

        }
    }
}