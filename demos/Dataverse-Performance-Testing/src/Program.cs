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
            ToPerformChoice.Title("How do you want to perform your upload test to Dataverse? [gray](all alllow for concurrency)[/]");
            ToPerformChoice.AddChoice("Standard uploads via POST request (one new record per HTTP request).");
            ToPerformChoice.AddChoice("Use [italic]CreateMultiple[/] to upload multiple records per HTTP request.");
            string ToPerform = AnsiConsole.Prompt(ToPerformChoice);

            //Handle
            if (ToPerform == "Standard uploads via POST request (one new record per HTTP request).")
            {
                AnsiConsole.MarkupLine("Great! I will upload new records to Dataverse with one record per API call.");

                //Get numnber of records to do in parallel?
                Console.Write("How many records do you want to upload at a time (in parallel)? Enter 1 if you want to do one-by-one. > ");
                int UploadBatch = 1;
                string? ParallelInput = Console.ReadLine();
                if (ParallelInput != null)
                {
                    UploadBatch = Convert.ToInt32(ParallelInput);
                }

                AnsiConsole.MarkupLine("I will upload new records to Dataverse with one record per API call, in batches of [bold]" + UploadBatch.ToString() + "[/] calls at a time (in parallel).");

                //Auth and validate
                DataverseService ds = await AuthenticateAndValidateAsync(auth);

                //Stats to control the upload process
                HttpClient hc = new HttpClient();

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
                DateTime UploadEnded = DateTime.UtcNow;

                //Print statistics
                TimeSpan TotalUploadTime = UploadEnded - UploadStarted;
                AnsiConsole.MarkupLine("[underline]Test #2 results @ batch of " + UploadBatch + "[/]");
                AnsiConsole.MarkupLine("[bold]" + RecordsUploaded.ToString("#,##0") + "[/] records uploaded in [bold]" + TotalUploadTime.TotalSeconds.ToString("#,##0") + " seconds[/]!");
            }
            else if (ToPerform == "Use [italic]CreateMultiple[/] to upload multiple records per HTTP request.")
            {
                //Get records per HTTP call
                int RecordsPerApiCall = 1;
                Console.Write("How many records to include in each API call? > ");
                string? input_RecordsPerHttpCall = Console.ReadLine();
                if (input_RecordsPerHttpCall != null)
                {
                    RecordsPerApiCall = Convert.ToInt32(input_RecordsPerHttpCall);
                }

                //Get API calls to make concurrently
                int ConcurrentApiCalls = 1;
                Console.Write("How many API calls to make concurrently? > ");
                string? input_ApiCallsInParallelStr = Console.ReadLine();
                if (input_ApiCallsInParallelStr != null)
                {
                    ConcurrentApiCalls = Convert.ToInt32(input_ApiCallsInParallelStr);
                }

                //Print
                AnsiConsole.MarkupLine("[blue]I will make [bold]" + ConcurrentApiCalls.ToString() + "[/] concurrent [italic]CreateMultiple[/] API calls, with each call containing [bold]" + RecordsPerApiCall.ToString() + "[/] records.[/]");

                //Authenticate to Dataverse - we are now ready to start
                DataverseService ds = await AuthenticateAndValidateAsync(auth);
                
                //Set up stuff
                HttpClient hc = new HttpClient();
                
                //Begin recording
                DateTime UploadStarted = DateTime.UtcNow;
                int RecordsUploaded = 0;
                string ErrorMessage = string.Empty;
                while (ErrorMessage == string.Empty)
                {
                    //Progress statistics
                    TimeSpan Remaining = auth.AccessTokenExpiresUtc - DateTime.UtcNow; //Estimate time remaining until token expires
                    TimeSpan Elapsed = DateTime.UtcNow - UploadStarted;
                    float RecordsPerMinute = Convert.ToSingle(RecordsUploaded) / Convert.ToSingle(Elapsed.TotalMinutes); //The avg records per minute so far
                    int UploadEstimate = Convert.ToInt32(RecordsUploaded + (RecordsPerMinute * Convert.ToSingle(Remaining.TotalMinutes))); //An estimate for how many records will be uploaded during this entire test, based on the trailing performance.

                    //Prepare list of HTTP requests to make (concurrently, if there are multiple)
                    List<Task<HttpResponseMessage>> UploadsToMake = new List<Task<HttpResponseMessage>>();
                    for (int u = 0; u < ConcurrentApiCalls; u++)
                    {
                        //Set up HTTP request
                        HttpRequestMessage req = new HttpRequestMessage();
                        req.Method = HttpMethod.Post;
                        req.RequestUri = new Uri(auth.Resource + "api/data/v9.2/timh_animals/Microsoft.Dynamics.CRM.CreateMultiple"); //CreateMultiple service
                        req.Headers.Add("Authorization", "Bearer " + auth.AccessToken);

                        //Construct body with the specified number of records
                        JArray targets = new JArray();
                        for (int rc = 0; rc < RecordsPerApiCall; rc++)
                        {
                            JObject NewAnimalRecord = Animal.Random().ForDataverseUpload();
                            NewAnimalRecord.Add("@odata.type", "Microsoft.Dynamics.CRM.timh_animal"); //Must add this to each record as per the CreateMultiple documentation.
                            targets.Add(NewAnimalRecord);
                        }
                        JObject body = new JObject();
                        body.Add("Targets", targets);
                        
                        //Append body
                        req.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");

                        //Add this HTTP request to the list of requests to do
                        UploadsToMake.Add(hc.SendAsync(req));
                    }

                    //Make all requests concurrently
                    AnsiConsole.Markup("[gray](" + Remaining.TotalMinutes.ToString("#,##0") + " mins remaining, est. " + UploadEstimate.ToString("#,##0") + " records @ " + RecordsPerMinute.ToString("#,##0.0") + " records/min)[/] "  + "POSTing [bold]" + UploadsToMake.Count.ToString("#,##0") + "[/] [italic]CreateMultiple[/] API calls with each call containing [bold]" + RecordsPerApiCall.ToString("#,##0") + "[/] records... ");
                    try
                    {
                        HttpResponseMessage[] responses = await Task.WhenAll(UploadsToMake);
                        AnsiConsole.Markup("[gray]completed... [/]");
                        
                        //Validate that all uploaded successfully
                        bool EveryApiCallWasSuccessful = true; //assume true
                        foreach (HttpResponseMessage response in responses)
                        {
                            if (response.StatusCode != System.Net.HttpStatusCode.OK) //The Dataverse API returns 200 OK to the CreateMultiple requests when they are successful
                            {
                                EveryApiCallWasSuccessful = false;
                                ErrorMessage = "Code: " + response.StatusCode.ToString() + " Msg: " + await response.Content.ReadAsStringAsync();
                                AnsiConsole.MarkupLine("[red]At least one API call from the last batch of API calls did NOT return 200 OK![/]");
                                AnsiConsole.WriteLine("Failed HTTP call: " + ErrorMessage);
                            }
                            else //This API call was successful (200 OK)!
                            {
                                string responseBodyStr = await response.Content.ReadAsStringAsync();
                                JObject responseBody = JObject.Parse(responseBodyStr);
                                JToken? Ids = responseBody.SelectToken("Ids");
                                if (Ids != null)
                                {
                                    JArray IdsJArray = (JArray)Ids;
                                    RecordsUploaded = RecordsUploaded = IdsJArray.Count; //Increment the # of records uploaded by the confirmed number of IDs (GUIDs) that come back, confirming record creation.
                                }
                                else //It is very unlikely for this to happen... because if it returned 200 OK, it will for sure return the correct response above where the "Ids" property contains GUIDS
                                {
                                    AnsiConsole.MarkupLine("[red]Unable to confirm upload of records via CreateMultiple! Property 'Ids' not found in response![/]");
                                    EveryApiCallWasSuccessful = false;
                                }
                            }
                        }

                        //If all was successful, print
                        if (EveryApiCallWasSuccessful)
                        {
                            AnsiConsole.MarkupLine("[green]Success![/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]At least partially failed![/]");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                        AnsiConsole.MarkupLine("[red]Error! Msg: " + ex.Message + "[/]");
                        Console.WriteLine(ex.StackTrace);
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