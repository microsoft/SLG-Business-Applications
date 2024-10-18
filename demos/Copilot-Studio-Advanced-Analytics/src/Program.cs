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
            string azblobconstr = "";

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

                //Azure Blob Connection string?
                JProperty? prop_azblob = jokeys.Property("azblob");
                if (prop_azblob != null)
                {
                    azblobconstr = prop_azblob.Value.ToString();
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

            //Get azure blob storage connection string
            if (azblobconstr == "")
            {
                Console.Write("Azure Blob Storage Connection String (optional) > ");
                string? s = Console.ReadLine();
                if (s != null)
                {
                    azblobconstr = s;
                }
            }

            //Parse Dataverse Credentials from JSON in file.
            DataverseAuthenticator auth = new DataverseAuthenticator();
            auth.Username = username;
            auth.Password = password;
            auth.Resource = resource;
            auth.ClientId = Guid.Parse("51f81489-12ee-4a9e-aaae-a2591f45987d"); //fill in default (standard) clientid
            AnsiConsole.MarkupLine("[italic]Dataverse credentials locked and loaded![/]");
            
            //Authenticate (call to Dataverse OAuth API and get access token that we can then use to make)
            AnsiConsole.Markup("[green][bold]Dataverse Auth[/][/]: Authenticating as '" + auth.Username + "' to environment '" + auth.Resource + "'... ");
            await auth.GetAccessTokenAsync();      
            AnsiConsole.MarkupLine("[italic][green]Authenticated![/][/]");     

            //Retrieve transcripts
            DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);
            AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving Copilot Studio transcripts... ");
            JArray transcripts = await ds.ReadAsync("conversationtranscripts");
            //JArray transcripts = JArray.Parse(System.IO.File.ReadAllText(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\Copilot-Studio-Advanced-Analytics\examples\conversationtranscripts.json")); //Collect from local, optionally
            AnsiConsole.MarkupLine("[italic][green]" + transcripts.Count.ToString("#,##0") + " transcripts retrieved![/][/]");

            //Retrieve bots
            AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving list of Copilot Studio bots... ");
            JArray bots = await ds.ReadAsync("bots");
            AnsiConsole.MarkupLine("[italic][green]" + bots.Count.ToString("#,##0") + " bots found![/][/]");

            //Parse them!
            CopilotStudioBot[] csbots = CopilotStudioBot.Parse(bots, transcripts);

            //Retrieve users
            AnsiConsole.Markup("[green][bold]Dataverse Read[/][/]: Retrieving System Users... ");
            TimHanewich.Dataverse.AdvancedRead.DataverseReadOperation dro = new TimHanewich.Dataverse.AdvancedRead.DataverseReadOperation();
            dro.TableIdentifier = "systemusers";
            dro.AddColumn("fullname");
            dro.AddColumn("systemuserid");
            dro.AddColumn("internalemailaddress");
            JArray systemusers = await ds.ReadAsync(dro);
            SystemUser[] users = SystemUser.Parse(systemusers);
            AnsiConsole.MarkupLine("[italic][green]" + users.Length.ToString("#,##0") + " users found![/][/]");

            Console.Clear(); //Clear the loading information stuff.
            
            //Infinite "what to do next?" loop
            while (true)
            {
                Markup title1 = new Markup(":robot: [bold][blue]Copilot Studio Advanced Analytics[/][/] :robot:");
                title1.Centered();
                AnsiConsole.Write(title1);

                Markup title2 = new Markup("[grey84]aka.ms/CSAA[/]");
                title2.Centered();
                AnsiConsole.Write(title2);

                Console.WriteLine();
                AnsiConsole.MarkupLine("[gray]Thank you for using [b]Copilot Studio Advanced Analytics (CSAA)[/]! CSAA is intended to showcase how professional developers can harness the power of the Dataverse API to extract session telemetry from Copilot Studio and transform this raw data into actionable insights. This application was developed by Tim Hanewich (aka.ms/timh) and the Microsoft U.S. State & Local Government Team.[/]");
                Console.WriteLine();

                //Data connections
                AnsiConsole.MarkupLine(":link: [green]Connected to Dataverse Environment [underline]'" + auth.Resource + "'[/] :link:[/]");

                
                //Prompt with options
                Console.WriteLine();
                SelectionPrompt<string> DoNext = new SelectionPrompt<string>();
                DoNext.Title("What do you want to do next?");
                DoNext.AddChoices("Review environment-level statistics");
                DoNext.AddChoice("See bot ownership breakdown");
                DoNext.AddChoice("Examine an individual bot's usage");
                DoNext.AddChoice("Review gen-AI content citations");
                DoNext.AddChoice("Archive session transcripts to Azure Blob Storage");
                DoNext.AddChoice("Exit");
                string DoNextSelection = AnsiConsole.Prompt(DoNext);

                //Clear the console after they select an option
                Console.Clear();


                //Handle decisions
                if (DoNextSelection == "Review environment-level statistics")
                {
                    AnsiConsole.MarkupLine("[bold][underline][navy]Environment-Level Analytics[/][/][/]");
                    AnsiConsole.MarkupLine("[gray]Copilot Studio Statistics for enviornment '" + auth.Resource + "':[/]");

                    Table t = new Table();

                    //Add Statistic column
                    TableColumn tc_statistic = new TableColumn("Statistic");
                    tc_statistic.Alignment = Justify.Left;
                    t.AddColumn(tc_statistic);

                    //Add Value column
                    TableColumn tc_value = new TableColumn("Value");
                    tc_value.Alignment = Justify.Right;
                    t.AddColumn(tc_value);

                    //Add bots
                    t.AddRow("# of Bots", csbots.Length.ToString());


                    //Add sessions
                    int sessionsc = 0;
                    foreach (CopilotStudioBot bot in csbots)
                    {
                        sessionsc = sessionsc + bot.Sessions.Length;
                    }
                    t.AddRow("# of Sessions", sessionsc.ToString("#,##0"));

                    //Add message
                    int messagesc = 0;
                    foreach (CopilotStudioBot bot in csbots)
                    {
                        foreach (CopilotStudioSession ses in bot.Sessions)
                        {
                            messagesc = messagesc + ses.Messages.Length;
                        }
                    }
                    t.AddRow("# of Messages, total", messagesc.ToString("#,##0"));

                    //Earliest Session
                    DateTime EarliestStart = DateTime.UtcNow;
                    foreach (CopilotStudioBot bot in csbots)
                    {
                        foreach (CopilotStudioSession session in bot.Sessions)
                        {
                            if (session.ConversationStart < EarliestStart)
                            {
                                EarliestStart = session.ConversationStart;
                            }
                        }
                    }
                    TimeSpan ts = DateTime.UtcNow - EarliestStart;
                    t.AddRow("Oldest session", EarliestStart.ToShortDateString() + " (" + ts.TotalDays.ToString("#,##0") + " days ago)");            

                    //Most recent session
                    DateTime MostRecentSession = EarliestStart.AddYears(-1);
                    foreach (CopilotStudioBot bot in csbots)
                    {
                        foreach (CopilotStudioSession session in bot.Sessions)
                        {
                            if (session.ConversationStart > MostRecentSession)
                            {
                                MostRecentSession = session.ConversationStart;
                            }
                        }
                    }
                    ts = DateTime.UtcNow - MostRecentSession;
                    t.AddRow("Most recent session", MostRecentSession.ToShortDateString() + " (" + ts.TotalDays.ToString("#,##0") + " days ago)");

                    //Messages per day avg
                    TimeSpan ElapsedTimeSinceFirstSession = DateTime.UtcNow - EarliestStart;
                    float msg_per_day = Convert.ToSingle(messagesc) / Convert.ToSingle(ElapsedTimeSinceFirstSession.TotalDays);
                    t.AddRow("Avg. Messages/Day", msg_per_day.ToString("#,##0.0"));

                    AnsiConsole.Write(t);
                }
                else if (DoNextSelection == "See bot ownership breakdown")
                {
                    AnsiConsole.MarkupLine("[bold][underline][navy]Bot Ownership Breakdown, by User[/][/][/]");
                    Console.WriteLine();

                    //Get bot ownership, by owner
                    Dictionary<SystemUser, int> BotOwnership = new Dictionary<SystemUser, int>();
                    foreach (SystemUser user in users)
                    {
                        //Count the # of bots they own
                        int bots_owned = 0;
                        foreach (CopilotStudioBot bot in csbots)
                        {
                            if (bot.Owner == user.SystemUserId)
                            {
                                bots_owned = bots_owned + 1;
                            }
                        }

                        //Log the # that they own if they have at least one
                        if (bots_owned > 0)
                        {
                            BotOwnership.Add(user, bots_owned);
                        }
                    }

                    //Sort those
                    Dictionary<SystemUser, int> BotOwnershipSorted = new Dictionary<SystemUser, int>();
                    while (BotOwnership.Count > 0)
                    {
                        KeyValuePair<SystemUser, int> Winner = BotOwnership.First();
                        foreach (KeyValuePair<SystemUser, int> item in BotOwnership)
                        {
                            if (item.Value > Winner.Value)
                            {
                                Winner = item;
                            }
                        }
                        BotOwnershipSorted.Add(Winner.Key, Winner.Value);
                        BotOwnership.Remove(Winner.Key);
                    }

                    //Write a Spectre.Console breakdown chart
                    BreakdownChart bc = new BreakdownChart();
                    bc.FullSize();


                    //Write the values
                    foreach (KeyValuePair<SystemUser, int> kvp in BotOwnershipSorted)
                    {
                        bc.AddItem(kvp.Key.FullName, kvp.Value, RandomColor());
                    }
                    AnsiConsole.Write(bc);



                }
                else if (DoNextSelection == "Examine an individual bot's usage")
                {
                    Tree root = new Tree("Bots in environment '" + auth.Resource + "'");
            
                    //Write a bit of info about each bot in a tree
                    foreach (CopilotStudioBot csbot in csbots)
                    {
                        TreeNode botnode = root.AddNode("[bold][blue]" + csbot.Name + "[/][/] [grey46][italic](" + csbot.SchemaName + ")[/][/]");

                        //Add # of sessions
                        TreeNode sessions = botnode.AddNode("[bold]" + csbot.Sessions.Length.ToString() + "[/] sessions");

                        //Add # of messages
                        TreeNode messages = botnode.AddNode("[bold]" + csbot.MessageCount.ToString("#,##0") + "[/] exchanged messages");

                        //Add # of citations
                        int citations = 0;
                        foreach (CopilotStudioSession ses in csbot.Sessions)
                        {
                            citations = citations + ses.Citations.Length;
                        }
                        TreeNode ncitations = botnode.AddNode("[bold]" + citations.ToString("#,##0") + "[/] citations");
                    }
                    AnsiConsole.Write(root);

                    //Offer a list of bots to pick from
                    if (csbots.Length > 0) //Only if there are bots
                    {
                        SelectionPrompt<string> BotPick = new SelectionPrompt<string>();
                        Console.WriteLine();
                        BotPick.Title("Which bot would you like to learn more about?");
                        foreach (CopilotStudioBot csbot in csbots)
                        {
                            BotPick.AddChoice("[bold][blue]" + csbot.Name + "[/][/] [grey46][italic](" + csbot.SchemaName + ")[/][/]");
                        }
                        BotPick.AddChoice("Go back to main menu");
                        string PickedBot = AnsiConsole.Prompt(BotPick);

                        //If they selected to go back to main menu, go back
                        if (PickedBot == "Go back to main menu")
                        {
                            break; //Will this work?
                        }

                        //Handle bot pick
                        //Find the bot they selected
                        foreach (CopilotStudioBot csbot in csbots)
                        {
                            if (PickedBot.Contains(csbot.SchemaName)) //If it is a match (using SchemaName to check)
                            {
                                //Clear
                                Console.Clear();

                                Tree broot = new Tree("Bot [bold][blue]" + csbot.Name + "[/][/] [grey46][italic](" + csbot.SchemaName + ")[/][/]");

                                //Add # of sessions
                                TreeNode sessions = broot.AddNode("[bold]" + csbot.Sessions.Length.ToString() + "[/] sessions");

                                //Add # of messages
                                TreeNode msgs = broot.AddNode("[bold]" + csbot.MessageCount.ToString("#,##0") + "[/] messages");

                                //Owner
                                foreach (SystemUser user in users)
                                {
                                    if (user.SystemUserId == csbot.Owner)
                                    {
                                        TreeNode ownernode = broot.AddNode("Owned by [bold]" + user.FullName + "[/] (" + user.Email + ")");
                                    }
                                }

                                //Oldest session
                                CopilotStudioSession? OldestSession = csbot.OldestSession;
                                if (OldestSession != null)
                                {
                                    broot.AddNode("Oldest Session: " + OldestSession.ConversationStart.ToShortDateString());
                                }

                                //Newest (most recent session)
                                CopilotStudioSession? MostRecentSession = csbot.NewestSession;
                                if (MostRecentSession != null)
                                {
                                    broot.AddNode("Newest (most recent) session: " + MostRecentSession.ConversationStart.ToShortDateString());
                                }

                                // Print the tree
                                AnsiConsole.Write(broot);

                                //Ask which transcript they want to analyze
                                if (csbot.Sessions.Length > 0) //If there are sessions
                                {
                                    SelectionPrompt<string> SessionSelection = new SelectionPrompt<string>();
                                    SessionSelection.Title("What session would you like to further analyze?");
                                    foreach (CopilotStudioSession ses in csbot.Sessions)
                                    {
                                        SessionSelection.AddChoice("Session '" + ses.SessionId.ToString() + "' from [bold]" + ses.ConversationStart.ToShortDateString() + "[/] - [bold]" + ses.MessageCount.ToString("#,##0") + "[/] exchanged messages");
                                    }
                                    Console.WriteLine(); //Buffer room
                                    string SessionSelectionChoice = AnsiConsole.Prompt(SessionSelection);

                                    //Handle what transcript they selected
                                    foreach (CopilotStudioSession ses in csbot.Sessions)
                                    {
                                        if (SessionSelectionChoice.Contains(ses.SessionId.ToString())) //If there is a match, using the ID showing up in the selection option
                                        {
                                            Console.Clear(); //Clear

                                            Console.WriteLine();
                                            AnsiConsole.MarkupLine("[bold][underline][blue]Transcript of session '" + ses.SessionId.ToString() + "' with bot '" + csbot.Name + "'[/][/][/]");

                                            //Create a table
                                            Table ChatTable = new Table();
                                            ChatTable.Width(Convert.ToInt32(Convert.ToSingle(Console.WindowWidth) / 1.5f));
                                            ChatTable.Border = TableBorder.Horizontal;

                                            //Add agent
                                            TableColumn tc_agent = new TableColumn("Agent");
                                            tc_agent.Alignment = Justify.Left;
                                            ChatTable.AddColumn(tc_agent);

                                            //Add human
                                            TableColumn tc_human = new TableColumn("Human");
                                            tc_human.Alignment = Justify.Right;
                                            ChatTable.AddColumn(tc_human);
                                            

                                            //Print every message
                                            foreach (CopilotStudioMessage msg in ses.Messages)
                                            {
                                                string txt_to_show = msg.Text.Replace("[", "[[").Replace("]", "]]");
                                                if (msg.Role == "human")
                                                {
                                                    ChatTable.AddRow("", txt_to_show);
                                                }
                                                else if (msg.Role == "bot")
                                                {
                                                    ChatTable.AddRow(txt_to_show, "");
                                                }

                                                //Add a buffer space
                                                ChatTable.AddRow("", "");
                                            }

                                            //Show the table!
                                            AnsiConsole.Write(ChatTable);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    




                }
                else if (DoNextSelection == "Review gen-AI content citations")
                {
                    //Collect all citations for this environment
                    List<CopilotTextCitation> AllCitations = new List<CopilotTextCitation>();
                    foreach (CopilotStudioBot csbot in csbots)
                    {
                        foreach (CopilotStudioSession ses in csbot.Sessions)
                        {
                            foreach (CopilotTextCitation ctt in ses.Citations)
                            {
                                AllCitations.Add(ctt);
                            }
                        }
                    }

                    //Collect # of times each site has been hit
                    Dictionary<string, int> citations = new Dictionary<string, int>();
                    foreach (CopilotTextCitation ctt in AllCitations)
                    {
                        string KeyToUse = ""; //Will either be using the URL or the Title. URL for websites, Title for documents and others (if the URL isn't there).
                        if (ctt.URL != null)
                        {
                            KeyToUse = ctt.URL;
                        }
                        else
                        {
                            KeyToUse = ctt.Title;
                        }

                        //Increment count
                        if (citations.ContainsKey(KeyToUse))
                        {
                            int curval = citations[KeyToUse];
                            curval = curval + 1;
                            citations[KeyToUse] = curval; //set to the new number (1 higher).
                        }
                        else
                        {
                            citations[KeyToUse] = 1;
                        }
                    }
                    
                    //Sort from most to least
                    Dictionary<string, int> citationsSorted = new Dictionary<string, int>();
                    while (citations.Count > 0)
                    {
                        KeyValuePair<string, int> winner = citations.First();
                        foreach (KeyValuePair<string, int> kvp in citations)
                        {
                            if (kvp.Value > winner.Value)
                            {
                                winner = kvp;
                            }
                        }
                        citationsSorted.Add(winner.Key, winner.Value);
                        citations.Remove(winner.Key);
                    }

                    //Write to bar chart
                    BarChart bc = new BarChart();
                    bc.Width = Console.WindowWidth;
                    bc.Label("[bold]Gen-AI Citations, by Source[/]");
                    bc.CenterLabel();
                    int on_footer_index = 1;
                    foreach (KeyValuePair<string, int> kvp in citationsSorted)
                    { 
                        bc.AddItem("[italic]Source " + on_footer_index.ToString() + "[/]", kvp.Value, RandomColor());
                        on_footer_index = on_footer_index + 1;
                    }
                    AnsiConsole.Write(bc);

                    //Select a source?
                    Console.WriteLine();
                    SelectionPrompt<string> SourceSelection = new SelectionPrompt<string>();
                    SourceSelection.Title("Which source would you like to research further?");
                    on_footer_index = 1;
                    foreach (KeyValuePair<string, int> kvp in citationsSorted)
                    {
                        SourceSelection.AddChoice("[italic][grey82]Source " + on_footer_index.ToString() + " = " + kvp.Key + "[/][/]");
                        on_footer_index = on_footer_index + 1;
                    }
                    string SourceSelectionChoice = AnsiConsole.Prompt(SourceSelection);

                    //Handle source selection #2
                    Console.Clear();

                    //Below is some pretty bad + inefficient coding.
                    //I could improve this but... I'll save it for later :)

                    //Find at least a single citation that this is shared with, so that we we have where it came from
                    string FROM_SOURCE_FOR_TITLE = "";
                    foreach (CopilotTextCitation ctc in AllCitations)
                    {
                        bool ThisIsOne = false;

                        //Title contained?
                        if (SourceSelectionChoice.EndsWith(ctc.Title + "[/][/]")) //Have to append the closing tags because those are in included in the selection (the selection was in markup)
                        {
                            ThisIsOne = true;
                        }

                        //URL contained?
                        if (ctc.URL != null)
                        {
                            if (SourceSelectionChoice.EndsWith(ctc.URL + "[/][/]")) //Have to append the closing tags because those are in included in the selection (the selection was in markup)
                            {
                                ThisIsOne = true;
                            }
                        }

                        if (ThisIsOne)
                        {
                            //Set up source we will cite as "from"
                            if (ctc.URL != null && ctc.URL != "")
                            {
                                FROM_SOURCE_FOR_TITLE = ctc.URL;
                            }
                            else
                            {
                                FROM_SOURCE_FOR_TITLE = ctc.Title;
                            }
                        }
                    }

                    //Show title
                    Markup sourcesTitle = new Markup("[bold][blue]Content Sourced from '" + FROM_SOURCE_FOR_TITLE + "'[/][/]");
                    sourcesTitle.Centered();
                    AnsiConsole.Write(sourcesTitle);
                    for (int t = 0; t < Console.WindowWidth; t++)
                    {
                        AnsiConsole.Markup("[gray]─[/]");
                    }

                    //Print each
                    foreach (CopilotTextCitation ctc in AllCitations)
                    {
                        bool ThisIsOne = false;

                        //Title contained?
                        if (SourceSelectionChoice.EndsWith(ctc.Title + "[/][/]")) //Have to append the closing tags because those are in included in the selection (the selection was in markup)
                        {
                            ThisIsOne = true;
                        }

                        //URL contained?
                        if (ctc.URL != null)
                        {
                            if (SourceSelectionChoice.EndsWith(ctc.URL + "[/][/]")) //Have to append the closing tags because those are in included in the selection (the selection was in markup)
                            {
                                ThisIsOne = true;
                            }
                        }

                        //If this is one of the citations w/ this particular source, list out info about it
                        if (ThisIsOne)
                        {
                            //Set up grid
                            Grid g = new Grid();
                            g.AddColumn();
                            g.AddColumn();
                            g.AddRow("[bold][underline]Sourced Content[/][/]", "[bold][underline]Generated Content[/][/]");

                            //Set up text + generated summary
                            ctc.Text = ctc.Text.Replace("[", "[[").Replace("]", "]]");
                            ctc.GeneratedSummary = ctc.GeneratedSummary.Replace("[", "[[").Replace("]", "]]");

                            //Set up source we will cite as "from"
                            string FromSource = "";
                            if (ctc.URL != null && ctc.URL != "")
                            {
                                FromSource = ctc.URL;
                            }
                            else
                            {
                                FromSource = ctc.Title;
                            }

                            //Print!
                            g.AddRow(ctc.Text + "\n\n" + "[italic]from '" + FromSource + "'[/]", ctc.GeneratedSummary);

                            //Print and add a space
                            AnsiConsole.Write(g);
                            Console.WriteLine();
                            for (int t = 0; t < Console.WindowWidth; t++)
                            {
                                AnsiConsole.Markup("[gray]─[/]");
                            }
                            Console.WriteLine();
                        }
                    }



                }
                else if (DoNextSelection == "Archive session transcripts to Azure Blob Storage")
                {

                    //If we don't have an azure con string yet, get it
                    while (azblobconstr == "")
                    {
                        Console.Write("What is your Azure Blob Storage connection string? > ");
                        string? i = Console.ReadLine();
                        if (i != null)
                        {
                            azblobconstr = i;
                        }
                    }

                    //CLEAR
                    Console.Clear();

                    //Count # of sessions
                    int sessions = 0;
                    foreach (CopilotStudioBot csbot in csbots)
                    {
                        sessions = sessions + csbot.Sessions.Length;
                    }

                    AnsiConsole.MarkupLine("[bold][underline][blue]Archive Transcripts to Azure Blob Storage[/][/][/]");
                    AnsiConsole.MarkupLine("I'm ready to begin archiving [bold]" + sessions.ToString() + "[/] Copilot Studio session transcripts to Azure Blob Storage!");
                    AnsiConsole.MarkupLine("This will only archive [bold]new[/] transcripts - any transcripts that are [italic]already[/] in Dataverse will be ignored.");
                    Console.WriteLine();
                    AnsiConsole.Markup("[italic]Ready when you are! Press enter to begin the upload process![/]");
                    Console.ReadLine();

                    //Authenticate and make container
                    BlobServiceClient bsc;
                    BlobContainerClient bcc;
                    try
                    {
                        bsc = new BlobServiceClient(azblobconstr);
                        bcc = bsc.GetBlobContainerClient("transcripts");
                        bcc.CreateIfNotExists();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine("[red]Authentication against Az Blob Storage did not work! Exception Message: " + ex.Message + "[/]");
                        Console.WriteLine();
                        AnsiConsole.MarkupLine("[italic]Enter to close error msg.[/]");
                        Console.ReadLine();
                        continue; //Go to next loop
                    }

                    //Loop through each session
                    int uploaded = 0;
                    int already_archived = 0;
                    int upload_errors = 0;
                    int ON_TICKER = 1;
                    foreach (CopilotStudioBot csbot in csbots)
                    {
                        foreach (CopilotStudioSession ses in csbot.Sessions)
                        {
                            AnsiConsole.Markup("[grey](" + ON_TICKER.ToString() + "/" + sessions.ToString() + ")[/] Uploading session '[bold]" + ses.SessionId.ToString() + "[/]' of bot '[bold]" + csbot.Name + "[/]' from [bold]" + ses.ConversationStart.ToShortDateString() + "[/]... ");

                            //Get blob client
                            string name = ses.SessionId.ToString().Replace("-", "") + ".json"; //File name that will be saved to blob
                            BlobClient bc = bcc.GetBlobClient(name);
                            if (bc.Exists() == false) //If the blob isn't already there
                            {
                                try
                                {
                                    //Prepare JSON object to upload
                                    JObject ToUpload = JObject.Parse(JsonConvert.SerializeObject(ses));
                                    JObject botinfo = new JObject();
                                    botinfo.Add("schemaName", csbot.SchemaName);
                                    botinfo.Add("name", csbot.Name);
                                    ToUpload.Add("bot", botinfo);

                                    //Upload
                                    string fcontent = ToUpload.ToString(Formatting.Indented);
                                    byte[] asbytes = Encoding.ASCII.GetBytes(fcontent);
                                    MemoryStream ms = new MemoryStream(asbytes);
                                    await bc.UploadAsync(ms);

                                    //Success msg
                                    uploaded = uploaded + 1;
                                    AnsiConsole.MarkupLine("[green]Success![/]");
                                }
                                catch (Exception ex)
                                {
                                    upload_errors = upload_errors + 1;   
                                    AnsiConsole.MarkupLine("[red]That did not work! Error msg: " + ex.Message + "[/]");
                                }                                
                            }
                            else
                            {
                                already_archived = already_archived + 1;
                                AnsiConsole.MarkupLine("[gray][italic](already archived)[/][/]");
                            }

                            //Increment on ticker
                            ON_TICKER = ON_TICKER + 1;
                        }
                    }

                    //Now that we are done, print completion
                    Console.WriteLine();
                    Console.WriteLine("Archiving of sessions to Azure Blob Storage complete!");
                    AnsiConsole.MarkupLine("- [bold][blue]" + uploaded.ToString("#,##0") + "[/][/] sessions uploaded");
                    AnsiConsole.MarkupLine("- [bold][blue]" + already_archived.ToString("#,##0") + "[/][/] sessions already uploaded previously");
                    AnsiConsole.MarkupLine("- [bold][blue]" + upload_errors.ToString("#,##0") + "[/][/] errors during upload");
                }
                else if (DoNextSelection == "Exit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]I'm not sure I understand that! Sorry![/]");
                }

                //Wait for them to press enter before clearing
                Console.WriteLine();
                AnsiConsole.Markup("[gray][italic]Enter to continue...[/][/]");
                Console.ReadLine();
                Console.Clear();
            }

            
            









            
        }
    
        public static Color RandomColor()
        {
            Random rand = new Random();
            int r = rand.Next(0, 256);
            int g = rand.Next(0, 256);
            int b = rand.Next(0, 256);
            return new Color((byte)r, (byte)g, (byte)b);
        }
    }
}