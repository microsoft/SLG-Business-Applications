using System;
using TimHanewich.AgentFramework;
using TIMEECore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TIMEEAPI
{
    [ApiController]
    [Route("chat")]
    public class Chat : ControllerBase
    {
        private ChatDB _cdb;

        public Chat(ChatDB cdb)
        {
            _cdb = cdb;
        }

        [HttpPost]
        public async Task Post()
        {

            //Ckeck for content-type header
            string? contenttype = Request.Headers["Content-Type"];
            if (contenttype == null || contenttype != "application/json")
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide data in JSON! Please ensure the data you provide is in JSON format and you specified the Content-Type header as application/json.");
                return;
            }

            //Get out body
            StreamReader sr = new StreamReader(Request.Body);
            string body = await sr.ReadToEndAsync();
            JObject jo;
            try
            {
                jo = JObject.Parse(body);
            }
            catch
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("There was an issue parsing the JSON out of the body. You must provide valid JSON!");
                return;
            }

            //Get key
            string key = "";
            JProperty? prop_key = jo.Property("key");
            if (prop_key == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide property 'key' in the request body.");
                return;
            }
            key = prop_key.Value.ToString();

            //Get message
            string msg = "";
            JProperty? prop_message = jo.Property("message");
            if (prop_message == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("You must provide property 'message' in the request body.");
                return;
            }
            msg = prop_message.Value.ToString();

            //Print both!
            Console.WriteLine("Necessary input parameters both collected:");
            Console.WriteLine("Key: " + key);
            Console.WriteLine("Message: " + msg);

            //Construct what we will return!
            string? ToReturn_Message = null;
            JObject? ToReturn_Timesheet = null;

            //Construct TIMEE Agent (Agent 1)
            Agent TIMEE = new Agent();
            TIMEE.Model = Settings.GetModelConnection();

            //Add the tool
            Tool generate_timesheet = new Tool();
            generate_timesheet.Name = "generate_timesheet";
            generate_timesheet.Description = "Generate a timesheet and display it to the user for approval.";
            generate_timesheet.Parameters.Add(new ToolInputParameter("description", "A summary description of the timesheet with all necessary details."));
            TIMEE.Tools.Add(generate_timesheet);

            //Retrieve messages (if there are any)
            Console.Write("Retrieving message history for '" + key + "'... ");
            List<Message> RetrievedMessages = _cdb.Retrieve(key);
            Console.WriteLine(RetrievedMessages.Count.ToString() + " messages retrieved.");
            TIMEE.Messages = RetrievedMessages;

            //If there are no messages, that means we are starting from a clean slate, so add the system prompt
            if (TIMEE.Messages.Count == 0)
            {
                TIMEE.Messages.Add(new Message(Role.system, Settings.TIMEESystemPrompt));
            }

            //Add the user message that was just provided
            TIMEE.Messages.Add(new Message(Role.user, msg));

            //Prompt!
            Console.Write("Prompting TIMEE... ");
            Message response;
            try
            {
                response = await TIMEE.PromptAsync();
                TIMEE.Messages.Add(response);
                Console.WriteLine("Success!");
            }
            catch (Exception ex)
            {
                string err = "Prompting of TIMEE failed! Msg: " + ex.Message;
                Console.WriteLine(err);
                Response.StatusCode = 500;
                await Response.WriteAsync(err);
                return;
            }

            //Is there a tool call involved? if so, invoke that right away
            if (response.ToolCalls.Length > 0)
            {

                //Handle tool calls
                foreach (ToolCall tc in response.ToolCalls)
                {
                    Console.WriteLine("Working on tool call of tool '" + tc.ToolName + "'... ");

                    Message ToolCallResponse = new Message();
                    ToolCallResponse.Role = Role.tool;
                    ToolCallResponse.ToolCallID = tc.ID;

                    if (tc.ToolName == generate_timesheet.Name)
                    {
                        //Get description parameter
                        JProperty? prop_description = tc.Arguments.Property("description");
                        if (prop_description == null)
                        {
                            string err = "TIMEE did not provide the timesheet description as parameter 'description' to the 'generate_timesheet' action";
                            Console.WriteLine(err);
                            Response.StatusCode = 500;
                            await Response.WriteAsync(err);
                            return;
                            throw new Exception();
                        }
                        string description = prop_description.Value.ToString();

                        //Prompt agent 2 to generate the timesheet!
                        Console.Write("Generating timesheet... ");
                        ToReturn_Timesheet = await TIMEECore.Program.GenerateTimesheetAsync(description);
                        Console.WriteLine("Done!");

                        //Respond to TIMEE that it has been made and sown
                        ToolCallResponse.Content = "The timesheet has been generated and shown to the user.";
                    }
                    else
                    {
                        string err = "Tool '" + tc.ToolName + "' not recognized!";
                        Console.WriteLine(err);
                        ToolCallResponse.Content = err;
                    }

                    //Add the tool response
                    TIMEE.Messages.Add(ToolCallResponse);
                }


                //Now re-prompt TIMEE
                Console.Write("Now that TIMEE has the tool call responses, re-prompting TIMEE... ");
                Message response2;
                try
                {
                    response2 = await TIMEE.PromptAsync();
                    TIMEE.Messages.Add(response2);
                    ToReturn_Message = response2.Content;
                    Console.WriteLine("Success!");
                }
                catch (Exception ex)
                {
                    string err = "Prompting of TIMEE failed! Msg: " + ex.Message;
                    Console.WriteLine(err);
                    Response.StatusCode = 500;
                    await Response.WriteAsync(err);
                    return;
                }
            }
            else //TIMEE did not want to call a function right now (generate timesheet). Instead it asked a follow up question.
            {
                ToReturn_Message = response.Content;
            }

            //Before returning, save this conversation history back to local DB storage
            Console.Write("Saving messages to local storage (state) before returning... ");
            _cdb.Save(key, TIMEE.Messages.ToArray());
            Console.WriteLine("Saved!");

            //Return!
            Console.WriteLine("Returning!");
            Response.StatusCode = 200;
            Response.Headers.Append("Content-Type", "application/json");
            JObject FullBodyToReturn = new JObject();
            FullBodyToReturn.Add("message", ToReturn_Message);
            FullBodyToReturn.Add("timesheet", ToReturn_Timesheet);
            await Response.WriteAsync(FullBodyToReturn.ToString());
        }

    }
}