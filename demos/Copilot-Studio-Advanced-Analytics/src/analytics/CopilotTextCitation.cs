using System;

namespace CopilotStudioAnalytics
{
    public class CopilotTextCitation
    {
        public string URL {get; set;} //URL of the webpage the citation come from
        public string Text {get; set;} //The text that was pulled down from the website and used as a reference for the answer to be generated on.        public CopilotTextCitation()
        public string GeneratedSummary {get; set;} //The content that Copilot generated using the text as a reference

        public CopilotTextCitation()
        {
            URL = "";
            Text = "";
            GeneratedSummary = "";
        }
    }
}