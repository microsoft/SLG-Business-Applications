using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CopilotStudioAnalytics
{
    public class CopilotStudioMessage
    {
        public string Role {get; set;} //i.e. "Copilot" or "User"
        public string Text {get; set;} //the message itself
        public DateTime TimeStamp {get; set;}

        public CopilotStudioMessage()
        {
            Role = "";
            Text = "";
        }

        public JObject ForDataverseUpload(Guid? session_id = null)
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("tsp_role", Role);
            ToReturn.Add("tsp_messagetext", Text);
            ToReturn.Add("tsp_timestamp", TimeStamp.ToString());

            //If they provided a session id, add the relationship
            if (session_id != null)
            {
                ToReturn.Add("tsp_CopilotSession@odata.bind", "tsp_copilotsessions(" + session_id.ToString() + ")");
            }
            
            return ToReturn;
        }
    }
}