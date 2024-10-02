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
            Console.WriteLine("Dataverse credentials received!");
            Console.Write("Authenticating as '" + auth.Username + "' to environment '" + auth.Resource + "'... ");
            
            //Authenticate (call to Dataverse OAuth API and get access token that we can then use to make)
            await auth.GetAccessTokenAsync();      
            Console.WriteLine("Authenticated!");     

            DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);
            Console.Write("Retrieving Copilot Studio transcripts... ");
            JArray transcripts = await ds.ReadAsync("conversationtranscripts");
            Console.WriteLine(transcripts.Count.ToString("#,##0") + " transcripts retrieved!");
            foreach (JObject transcipt in transcripts)
            {

                //Print transcript ID
                JProperty? id = transcipt.Property("conversationtranscriptid");
                if (id != null)
                {
                    Console.WriteLine("--- TRANSCRIPT " + id.Value.ToString() + "---");
                }

                //Print transcript start time
                JProperty? starttime = transcipt.Property("conversationstarttime");
                if (starttime != null)
                {
                    DateTime st = DateTime.Parse(starttime.Value.ToString());
                    Console.WriteLine("Conversation @ " + st.ToString());
                }

                //Print Bot ID
                JProperty? pmetadata = transcipt.Property("metadata");
                if (pmetadata != null)
                {
                    JObject metadata = JObject.Parse(pmetadata.Value.ToString());
                    
                    //Bot ID
                    string BotId = "";
                    JProperty? pBotId = metadata.Property("BotId");
                    if (pBotId != null)
                    {
                        BotId = pBotId.Value.ToString();
                    }

                    //Bot Name
                    string BotName = "";
                    JProperty? pBotName = metadata.Property("BotName");
                    if (pBotName != null)
                    {
                        BotName = pBotName.Value.ToString();
                    }

                    //Print
                    Console.WriteLine("Bot: '" + BotName + "' (" + BotId + ")");
                }


                //Parse the "content" property, which contains the transcript data.
                JProperty? pcontent = transcipt.Property("content");
                if (pcontent != null)
                {
                    JObject content = JObject.Parse(pcontent.Value.ToString());
                    JArray? activities = content.SelectToken("activities") as JArray;
                    if (activities != null)
                    {
                        foreach (JObject activity in activities)
                        {
                            JProperty? type = activity.Property("type");
                            if (type != null)
                            {
                                string ToPrint = type.Value.ToString();

                                //Handle how to print these uniquely
                                if (type.Value.ToString() == "message") //If it is a message
                                {
                                    
                                    //From who?
                                    JToken? from_role = activity.SelectToken("from.role");
                                    if (from_role != null)
                                    {
                                        if (from_role.Value<int>() == 0)
                                        {
                                            ToPrint = "Copilot: ";
                                        }      
                                        else
                                        {
                                            ToPrint = "User: ";
                                        }                                  
                                    }


                                    //Get text
                                    JProperty? text = activity.Property("text");
                                    if (text != null)
                                    {
                                        ToPrint = ToPrint + text.Value.ToString();
                                    }
                                }
                                else if (type.Value.ToString() == "event")//If it is an event
                                {
                                    //Get name
                                    JProperty? name = activity.Property("name");
                                    if (name != null)
                                    {
                                        ToPrint = "\t" + "event - " + name.Value.ToString();

                                        //If name of event is "DialogTracing", show ActionType
                                        if (name.Value.ToString() == "DialogTracing")
                                        {
                                            JToken? actionType = activity.SelectToken("value.actions[0].actionType");
                                            if (actionType != null)
                                            {
                                                ToPrint = ToPrint + ", actionType = " + actionType.Value<string>();
                                            }
                                        }
                                        else //If it isn't dialog tracing, just show the channelId
                                        {
                                            JProperty? channelId = activity.Property("channelId");
                                            if (channelId != null)
                                            {
                                                ToPrint = ToPrint + " on channel " + channelId.Value.ToString();
                                            }
                                        }
                                    }
                                }
                                else if (type.Value.ToString() == "trace")
                                {
                                    JProperty? valueType = activity.Property("valueType");
                                    if (valueType != null)
                                    {
                                        ToPrint = "\t" + "trace - " + valueType.Value.ToString();

                                        //If it is a SessionInfo, try to get what the outcome was
                                        JToken? outcome = activity.SelectToken("value.outcome");
                                        if (outcome != null)
                                        {
                                            ToPrint = ToPrint + " - outcome of chat was '" + outcome.Value<string>() + "'";
                                        }
                                    }
                                }
                                else
                                {
                                    ToPrint = type.Value.ToString();
                                }

                                

                                Console.WriteLine(ToPrint);
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
    }
}