using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CopilotStudioAnalytics
{
    public class CopilotStudioSession
    {
        public Guid SessionId {get; set;}
        public DateTime ConversationStart {get; set;}
        public DateTime ConversationEnd {get; set;}
        public Guid BotId {get; set;} //GUID ID of the Copilot Studio bot
        public string BotName {get; set;} //schema name of the bot
        public string Outcome {get; set;} //From the final "SessionInfo"
        public int TurnCount {get; set;} //From the final "SessionInfo"
        public CopilotStudioMessage[] Messages {get; set;}

        public CopilotStudioSession()
        {
            BotName = "";
            Outcome = "";
            Messages = new CopilotStudioMessage[0]; //blank array
        }

        public static CopilotStudioSession[] Parse(JArray conversation_transcripts, JArray bots)
        {
            List<CopilotStudioSession> ToReturn = new List<CopilotStudioSession>();
            foreach (JObject transcipt in conversation_transcripts)
            {

                //Create this session data holds
                CopilotStudioSession ThisSession = new CopilotStudioSession();
                List<CopilotStudioMessage> ThisSessionsMessages = new List<CopilotStudioMessage>();

                //Transcript ID
                JProperty? id = transcipt.Property("conversationtranscriptid");
                if (id != null)
                {
                    ThisSession.SessionId = Guid.Parse(id.Value.ToString());
                }

                //Print transcript start time
                // JProperty? starttime = transcipt.Property("conversationstarttime");
                // if (starttime != null)
                // {
                //     DateTime st = DateTime.Parse(starttime.Value.ToString());
                //     Console.WriteLine("Conversation @ " + st.ToString());
                // }

                //Save Bot Id, Bot Name
                JProperty? pmetadata = transcipt.Property("metadata");
                if (pmetadata != null)
                {
                    JObject metadata = JObject.Parse(pmetadata.Value.ToString());
                    
                    //Bot ID
                    JProperty? pBotId = metadata.Property("BotId");
                    if (pBotId != null)
                    {
                        ThisSession.BotId = Guid.Parse(pBotId.Value.ToString());
                    }

                    //Bot Name
                    JProperty? pBotName = metadata.Property("BotName");
                    if (pBotName != null)
                    {
                        ThisSession.BotName = pBotName.Value.ToString();
                    }

                    //Bot friendly name
                    //Fish through other bot list.
                }


                //Parse the "content" property, which contains the transcript data.
                JProperty? pcontent = transcipt.Property("content");
                if (pcontent != null)
                {
                    JObject content = JObject.Parse(pcontent.Value.ToString());
                    JArray? activities = content.SelectToken("activities") as JArray;
                    if (activities != null)
                    {
                        //Loop through each activity.
                        foreach (JObject activity in activities)
                        {

                            //Is there an "outcome" property?
                            JToken? outcome = activity.SelectToken("value.outcome");
                            if (outcome != null)
                            {
                                string? outcomes = outcome.Value<string>();
                                if (outcomes != null)
                                {
                                    ThisSession.Outcome = outcomes;
                                }
                            }

                            //Is there a "startTimeUtc" property?
                            JToken? startTimeUtc = activity.SelectToken("value.startTimeUtc");
                            if (startTimeUtc != null)
                            {
                                string? startTimeUtcs = startTimeUtc.Value<string>();
                                if (startTimeUtcs != null)
                                {
                                    ThisSession.ConversationStart = DateTime.Parse(startTimeUtcs);
                                }
                            }

                            //Is there a "endTimeUtc" property?
                            JToken? endTimeUtc = activity.SelectToken("value.endTimeUtc");
                            if (endTimeUtc != null)
                            {
                                string? endTimeUtcs = endTimeUtc.Value<string>();
                                if (endTimeUtcs != null)
                                {
                                    ThisSession.ConversationEnd = DateTime.Parse(endTimeUtcs);
                                }
                            }  

                            //Is there a "turnCount" property?
                            JToken? turnCount = activity.SelectToken("value.turnCount");
                            if (turnCount != null)
                            {
                                ThisSession.TurnCount = turnCount.Value<int>();
                            }

                            //Get the "type" property
                            JProperty? prop_type = activity.Property("type");
                            if (prop_type != null)
                            {
                                if (prop_type.Value.ToString() == "message") //It is an exchanged message between the bot and user
                                {
                                    CopilotStudioMessage msg = new CopilotStudioMessage();

                                    //Get role
                                    JToken? role = activity.SelectToken("from.role");
                                    if (role != null)
                                    {
                                        int rolei = role.Value<int>();
                                        if (rolei == 0)
                                        {
                                            msg.Role = "bot";
                                        }
                                        else if (rolei == 1)
                                        {
                                            msg.Role = "human";
                                        }
                                        else
                                        {
                                            msg.Role = "?";
                                        }
                                    }

                                    //Get text
                                    JProperty? text = activity.Property("text");
                                    if (text != null)
                                    {
                                        msg.Text = text.Value.ToString();
                                    }

                                    //Add to message list
                                    ThisSessionsMessages.Add(msg);
                                }
                            }



                        }
                    }
                }
            
                //Add this session to the list of items to return
                ThisSession.Messages = ThisSessionsMessages.ToArray(); //First, append the messages to it
                ToReturn.Add(ThisSession); //Save it in the array that will be returned.
            }
            return ToReturn.ToArray();
        }
    }
}