using System;
using TimHanewich.Dataverse;
using Spectre.Console;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimHanewich.Dataverse.Metadata;
using System.Text;

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
            ToPerformChoice.AddChoice("Test #2 - Upload one-by-one, but in parallel");
            string ToPerform = AnsiConsole.Prompt(ToPerformChoice);

            //Handle
            if (ToPerform == "Test #1 - Upload one-by-one")
            {
                //Authenticate and validate
                DataverseService ds = await AuthenticateAndValidateAsync(auth);
                
                //Begin recording
                DateTime UploadStarted = DateTime.UtcNow;
                int RecordsUploaded = 0;
                string ErrorMessage = string.Empty;
                while (ErrorMessage == string.Empty)
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
                        ErrorMessage = ex.Message;
                        AnsiConsole.MarkupLine("[red]Error! Msg: " + ex.Message + "[/]");
                    }
                }
                DateTime UploadEnded = DateTime.UtcNow;

                //Print statistics
                TimeSpan TotalUploadTime = UploadEnded - UploadStarted;
                AnsiConsole.MarkupLine("[bold]" + RecordsUploaded.ToString("#,##0") + "[/] records uploaded in [bold]" + TotalUploadTime.TotalSeconds.ToString("#,##0") + " seconds[/]!");
            }
            else if (ToPerform == "Test #2 - Upload one-by-one, but in parallel")
            {
                //Auth and validate
                DataverseService ds = await AuthenticateAndValidateAsync(auth);

                //Stats to control the upload process
                HttpClient hc = new HttpClient();
                int UploadBatch = 52; //How many uploads to do concurrently


                //Begin recording
                DateTime UploadStarted = DateTime.UtcNow;
                int RecordsUploaded = 0;
                string ErrorMessage = string.Empty;
                while (ErrorMessage == string.Empty)
                {
                    TimeSpan Remaining = auth.AccessTokenExpiresUtc - DateTime.UtcNow; //Estimate time remaining until token expires
                    TimeSpan Elapsed = DateTime.UtcNow - UploadStarted;
                    float RecordsPerMinute = Convert.ToSingle(RecordsUploaded) / Convert.ToSingle(Elapsed.TotalMinutes); //The avg records per minute so far
                    int UploadEstimate = Convert.ToInt32(RecordsUploaded + (RecordsPerMinute * Convert.ToSingle(Remaining.TotalMinutes))); //An estimate for how many records will be uploaded during this entire test, based on the trailing performance.

                    //Create list of tasks
                    List<Task<HttpResponseMessage>> UploadsToDo = new List<Task<HttpResponseMessage>>();
                    for (int i = 0; i < UploadBatch; i++)
                    {
                        Animal a = Animal.Random();
                        HttpRequestMessage req = new HttpRequestMessage();
                        req.Method = HttpMethod.Post;
                        req.RequestUri = new Uri(auth.Resource + "api/data/v9.2/timh_animals");
                        req.Headers.Add("Authorization", "Bearer " + auth.AccessToken);
                        req.Content = new StringContent(a.ForDataverseUpload().ToString(), Encoding.UTF8, "application/json");
                        UploadsToDo.Add(hc.SendAsync(req));
                    }

                    //Wait for all the tasks to be upload
                    AnsiConsole.Markup("[gray](" + Remaining.TotalMinutes.ToString("#,##0") + " mins remaining, est. " + UploadEstimate.ToString("#,##0") + " records @ " + RecordsPerMinute.ToString("#,##0.0") + " records/min)[/] "  + "POSTing " + UploadsToDo.Count.ToString("#,##0") + " concurrent upload calls... ");
                    try
                    {
                        //Call!
                        HttpResponseMessage[] responses = await Task.WhenAll(UploadsToDo);

                        //Check for errors
                        bool AllUploadedSuccessfully = true; //Assume true
                        foreach (HttpResponseMessage resp in responses)
                        {
                            if (resp.StatusCode != System.Net.HttpStatusCode.NoContent) //NoContent is what the Dataverse API returns when creation was successful
                            {
                                AllUploadedSuccessfully = false;
                                ErrorMessage = "Status Code: " + resp.StatusCode.ToString() + ", Msg: " + await resp.Content.ReadAsStringAsync();
                                AnsiConsole.MarkupLine("[red]At least one upload from this batch failed! " + ErrorMessage + "[/]");
                            }
                            else //It was successful
                            {
                                RecordsUploaded = RecordsUploaded + 1; //Count it!
                            }
                        }
                        
                        //Success?
                        if (AllUploadedSuccessfully)
                        {
                            AnsiConsole.MarkupLine("[green]Success![/]");
                        } 
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine("[red]Error! Msg: " + ex.Message + "[/]");
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]I do not know how to handle selection '" + ToPerform + "'![/]");
            }

        }


        //Authenticates against Dataverse and verifies that the timh_animals table exists.
        public static async Task<DataverseService> AuthenticateAndValidateAsync(DataverseAuthenticator auth)
        {
            //Authenticate against Dataverse
            AnsiConsole.Markup("Authenticating against Dataverse... ");
            await auth.GetAccessTokenAsync();
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

            //Return
            return ds;
        }
    }
}