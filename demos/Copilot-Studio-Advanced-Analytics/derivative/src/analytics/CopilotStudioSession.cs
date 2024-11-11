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

    }
}