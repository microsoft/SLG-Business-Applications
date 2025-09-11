using System;
using Newtonsoft.Json.Linq;

namespace CopilotStudioAnalytics
{
    public class CopilotStudioBot
    {
        public string Name {get; set;} //Display name
        public string SchemaName {get; set;} //Schema name (unique)
        public Guid Owner {get; set;} //GUID of the SystemUser (user) that owns this bot.
        public DateTime CreatedOn {get; set;}
        public CopilotStudioSession[] Sessions {get; set;} //All sessions

        CopilotStudioBot()
        {
            Name = "";
            SchemaName = "";
            Sessions = new CopilotStudioSession[]{};
        }

        public static CopilotStudioBot[] Parse(JArray bots, JArray sessions)
        {
            List<CopilotStudioBot> ToReturn = new List<CopilotStudioBot>();

            //Parse each bot
            foreach (JObject bot in bots)
            {
                CopilotStudioBot nbot = new CopilotStudioBot();
                
                //Get name
                JProperty? prop_name = bot.Property("name");
                if (prop_name != null)
                {
                    nbot.Name = prop_name.Value.ToString();
                }

                //Get schema name
                JProperty? prop_schemaname = bot.Property("schemaname");
                if (prop_schemaname != null)
                {
                    nbot.SchemaName = prop_schemaname.Value.ToString();
                }

                //Get CreatedOn
                JProperty? prop_createdon = bot.Property("createdon");
                if (prop_createdon != null)
                {
                    nbot.CreatedOn = DateTime.Parse(prop_createdon.Value.ToString());
                }

                //Get owner
                JProperty? owner = bot.Property("_ownerid_value");
                if (owner != null)
                {
                    nbot.Owner = Guid.Parse(owner.Value.ToString());
                }

                //Parse and collected transcripts (sessions) that belong to this bot
                List<CopilotStudioSession> RelatedSessions = new List<CopilotStudioSession>();
                foreach (JObject transcipt in sessions)
                {

                    //Retrieve bot name
                    string BOT_NAME = "";
                    JProperty? pmetadata = transcipt.Property("metadata");
                    if (pmetadata != null)
                    {
                        JObject metadata = JObject.Parse(pmetadata.Value.ToString());
                    

                        //Bot Name (schema name)
                        JProperty? pBotName = metadata.Property("BotName");
                        if (pBotName != null)
                        {
                            BOT_NAME = pBotName.Value.ToString();
                        }
                    }

                    //If the BotName (Schemaname) of this transcript is the same as the bot we are on right now, parse it and save it to this bot!
                    if (BOT_NAME == nbot.SchemaName)
                    {
                        //Create this session data holds
                        CopilotStudioSession ThisSession = new CopilotStudioSession();
                        List<CopilotStudioMessage> ThisSessionsMessages = new List<CopilotStudioMessage>();
                        List<CopilotTextCitation> ThisSessionsCitations = new List<CopilotTextCitation>();

                        //Transcript ID
                        JProperty? id = transcipt.Property("conversationtranscriptid");
                        if (id != null)
                        {
                            ThisSession.SessionId = Guid.Parse(id.Value.ToString());
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

                                    //Was this GPT generated content from a website?
                                    JToken? TextCitations = activity.SelectToken("channelData.pva:gpt-feedback.summarizationOpenAIResponse.result.textCitations");
                                    if (TextCitations != null)
                                    {

                                        //If text citations are there, there is also a "summary" in the object above. Grab that, that is the generated messaged
                                        JToken? summary = activity.SelectToken("channelData.pva:gpt-feedback.summarizationOpenAIResponse.result.summary");
                                        string summary_str = "";
                                        if (summary != null)
                                        {
                                            string? s = summary.Value<string>();
                                            if (s != null)
                                            {
                                                summary_str = s;
                                            } 
                                        }

                                        //Loop through and save each text citation
                                        foreach (JObject tcitation in TextCitations)
                                        {
                                            CopilotTextCitation ctc = new CopilotTextCitation();
                                            
                                            //Get title
                                            JProperty? prop_title = tcitation.Property("title");
                                            if (prop_title != null)
                                            {
                                                ctc.Title = prop_title.Value.ToString().Replace("%20", " ");
                                            }

                                            //Get URL (only if it exists, it is only there for website content, and only if it is not "")
                                            JProperty? prop_url = tcitation.Property("url");
                                            if (prop_url != null)
                                            {
                                                if (prop_url.Value.ToString() != "")
                                                {
                                                    ctc.URL = prop_url.Value.ToString();
                                                }
                                            }
                                            else
                                            {
                                                ctc.URL = null;
                                            }
                                            
                                            //Get text
                                            JProperty? prop_text = tcitation.Property("text");
                                            if (prop_text != null)
                                            {
                                                ctc.Text = prop_text.Value.ToString();
                                            }

                                            //Plug in generated summary
                                            ctc.GeneratedSummary = summary_str;
                                            
                                            //Add it
                                            ThisSessionsCitations.Add(ctc);
                                        }
                                    }
                                    

                                    //Get the "type" property
                                    //This is for messages
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
                        ThisSession.Citations = ThisSessionsCitations.ToArray();
                        RelatedSessions.Add(ThisSession);
                    }
                    nbot.Sessions = RelatedSessions.ToArray(); //Add all of the collected sessions.
                }
            
                //Add the bot to the list of bots to return
                ToReturn.Add(nbot);
            }   

            //Return the bots
            return ToReturn.ToArray();
        }
    
        public string PreviewText()
        {

            //Count # of messages across sessions
            int msgs = 0;
            foreach (CopilotStudioSession ses in Sessions)
            {
                msgs = msgs + ses.TurnCount;
            }

            return Name + " (" + SchemaName + ") - " + Sessions.Length.ToString("#,##0") + " sessions, " + msgs.ToString("#,##0") + " messages";
        }
    
        public int MessageCount
        {
            get
            {
                int c = 0;
                foreach (CopilotStudioSession session in Sessions)
                {
                    c = c + session.MessageCount;
                }
                return c;
            }
        }

        //Returns the oldest session
        public CopilotStudioSession? OldestSession
        {
            get
            {
                if (Sessions.Length == 0)
                {
                    return null;
                }
                CopilotStudioSession Winner = Sessions[0];
                foreach (CopilotStudioSession ses in Sessions)
                {
                    if (ses.ConversationStart < Winner.ConversationStart)
                    {
                        Winner = ses; 
                    }
                }
                return Winner;
            }
        }

        //Returns the most recent session
        public CopilotStudioSession? NewestSession 
        {
            get
            {
                if (Sessions.Length == 0)
                {
                    return null;
                }
                CopilotStudioSession Winner = Sessions[0];
                foreach (CopilotStudioSession ses in Sessions)
                {
                    if (ses.ConversationStart > Winner.ConversationStart)
                    {
                        Winner = ses; 
                    }
                }
                return Winner;
            }
        }
    }
}