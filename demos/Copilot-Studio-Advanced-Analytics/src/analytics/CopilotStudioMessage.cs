using System;

namespace CopilotStudioAnalytics
{
    public class CopilotStudioMessage
    {
        public string Role {get; set;} //i.e. "Copilot" or "User"
        public string Text {get; set;} //the message itself

        public CopilotStudioMessage()
        {
            Role = "";
            Text = "";
        }
    }
}