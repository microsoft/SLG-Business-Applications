using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CopilotStudioAnalytics
{
    public class CopilotStudioSession
    {
        public Guid BotID {get; set;} //The ID (GUID) of the bot this transcript corresponds with
        public Guid SessionId {get; set;}
        public DateTime ConversationStart {get; set;}
        public DateTime ConversationEnd {get; set;}
        public string Outcome {get; set;} //From the final "SessionInfo"
        public int TurnCount {get; set;} //From the final "SessionInfo"
        public CopilotStudioMessage[] Messages {get; set;}
        public CopilotTextCitation[] Citations {get; set;}

        public CopilotStudioSession()
        {
            Outcome = "";
            Messages = new CopilotStudioMessage[]{}; //blank array
            Citations = new CopilotTextCitation[]{}; //blank array
        }

        public int MessageCount
        {
            get
            {
                return Messages.Length;
            }
        }

        public JObject ForDataverseUpload(CopilotStudioBot? bot_it_is_for = null)
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("tsp_copilotsessionid", SessionId); //This is the primary key... we want the primary key (GUID) of the tsp_copilotsessions record to MATCH the ID of the associated conversationtranscripts record.
            ToReturn.Add("tsp_conversationstart", ConversationStart.ToString());
            ToReturn.Add("tsp_conversationend", ConversationEnd.ToString());
            ToReturn.Add("tsp_turncount", TurnCount);
            ToReturn.Add("tsp_outcome", Outcome);

            //Add primary name field
            if (bot_it_is_for == null)
            {
                ToReturn.Add("tsp_copilotstudiosessionid", MessageCount.ToString("#,##0") + " messages from " + ConversationStart.ToShortDateString()); //This is the primary column, not primary key
            }
            else //They provided the bot it came from
            {
                ToReturn.Add("tsp_copilotstudiosessionid", MessageCount.ToString("#,##0") + " messages with '" + bot_it_is_for.Name + "' from " + ConversationStart.ToShortDateString()); //This is the primary column, not primary key
            }

            //Form relationship to parent entity, the "bot" table
            if (bot_it_is_for != null)
            {
                ToReturn.Add("tsp_Copilot@odata.bind", "bots(" + bot_it_is_for.BotID.ToString() + ")");
            }
            
            return ToReturn;
        }

    }
}