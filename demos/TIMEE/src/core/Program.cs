using System;
using TimHanewich.AgentFramework;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TIMEE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Go().Wait();
        }


        public static async Task Go()
        {
            Agent TIMEE = new Agent();
            TIMEE.Model = new AzureOpenAICredentials("https://ai-testaistudio030597089470.openai.azure.com/openai/deployments/aida-gpt-4.1/chat/completions?api-version=2025-01-01-preview", "52799f2a98ac497baddc5da03e6c3cdd");

            //Add the tool
            Tool generate_timesheet = new Tool();
            generate_timesheet.Name = "generate_timesheet";
            generate_timesheet.Description = "Generate a timesheet and display it to the user for approval.";
            generate_timesheet.Parameters.Add(new ToolInputParameter("description", "A summary description of the timesheet with all necessary details."));
            TIMEE.Tools.Add(generate_timesheet);

            //Add system prompt
            TIMEE.Messages.Add(new Message(Role.system, System.IO.File.ReadAllText(@"C:\Users\timh\OneDrive - Microsoft\Stretch Projects\AI POC Teasers (CHASE IRIS)\TIMEE\prompts\agent1.md")));

            while (true)
            {

                //Collect user input
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (input == null)
                {
                    throw new Exception("You must enter something in!!");
                }

                //Add it as a message
                Message userMSG = new Message(Role.user, input);
                TIMEE.Messages.Add(userMSG);

            //Prompt the model
            PromptModel:
                Console.WriteLine();
                Console.Write("Thinking...");
                Message response = await TIMEE.PromptAsync();
                TIMEE.Messages.Add(response);
                Console.WriteLine("complete!");


                //Handle content
                if (response.Content != null)
                {
                    Console.WriteLine("TIMEE: " + response.Content);
                }

                //Handle tool calls
                if (response.ToolCalls.Length > 0)
                {
                    foreach (ToolCall tc in response.ToolCalls)
                    {
                        Console.WriteLine("Working on tool call of tool '" + tc.ToolName + "'...");

                        Message ToolCallResponse = new Message();
                        ToolCallResponse.Role = Role.tool;
                        ToolCallResponse.ToolCallID = tc.ID;

                        if (tc.ToolName == generate_timesheet.Name)
                        {
                            //Get description parameter
                            JProperty? prop_description = tc.Arguments.Property("description");
                            if (prop_description == null)
                            {
                                throw new Exception("TIMEE did not provide the timesheet description as parameter 'description' to the 'generate_timesheet' action");
                            }
                            string description = prop_description.Value.ToString();

                            //Prompt agent 2 to generate the timesheet!
                            Console.Write("Generating timesheet... ");
                            JObject timesheet = await GenerateTimesheetAsync(description);
                            Console.WriteLine("Done!");

                            //Show the timesheet
                            Console.WriteLine(timesheet.ToString());

                            //Respond to TIMEE that it has been made and sown
                            ToolCallResponse.Content = "The timesheet has been generated and shown to the user.";
                        }
                        else
                        {
                            string err = "Tool '" + tc.ToolName + "' not recognized!";
                            Console.WriteLine(err);
                            ToolCallResponse.Content = err;
                        }

                        //Add it
                        TIMEE.Messages.Add(ToolCallResponse);
                    }

                    //Go right back to prompting model
                    goto PromptModel;
                }

                
            }

        }

        public static async Task<JObject> GenerateTimesheetAsync(string description)
        {
            Agent a = new Agent();
            a.Model = new AzureOpenAICredentials("https://ai-testaistudio030597089470.openai.azure.com/openai/deployments/aida-gpt-4.1/chat/completions?api-version=2025-01-01-preview", "52799f2a98ac497baddc5da03e6c3cdd");

            //Add system prompt
            a.Messages.Add(new Message(Role.system, System.IO.File.ReadAllText(@"C:\Users\timh\OneDrive - Microsoft\Stretch Projects\AI POC Teasers (CHASE IRIS)\TIMEE\prompts\agent2.md")));

            //Add user prompt
            a.Messages.Add(new Message(Role.user, "The following is a description of a timesheet. Please generate provide this in JSON format as you have been instructed to: \n\n" + description));


            //Prompt
            Message response = await a.PromptAsync(json_mode: true);
            if (response.Content == null)
            {
                throw new Exception("Timesheet Generation Agent did not generate content!");
            }
            try
            {
                JObject ToReturn = JObject.Parse(response.Content);
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception("Error! It appears the Timesheet Generation Agent did not return proper JSON. Msg: " + ex.Message);
            }
        }


    }
}