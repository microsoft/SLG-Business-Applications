using System;

namespace CopilotStudioAnalytics
{
    public class CopilotTextCitation
    {
        public string Title {get; set;} //URL of the webpage the citation come from, or the PDF that it came from, etc. Whatever it is.
        public string Text {get; set;} //The text that was pulled down from the website and used as a reference for the answer to be generated on.        public CopilotTextCitation()
        public string GeneratedSummary {get; set;} //The content that Copilot generated using the text as a reference
        public string? URL {get; set;} //If it was retrieved from a website, this will contain the URL.

        public CopilotTextCitation()
        {
            Title = "";
            Text = "";
            GeneratedSummary = "";
            URL = null;
        }
    }
}