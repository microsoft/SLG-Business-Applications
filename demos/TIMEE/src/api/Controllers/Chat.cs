using System;
using TimHanewich.AgentFramework;
using TIMEE;
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

            //Construct TIMEE Agent (Agent 1)
            Agent TIMEE = new Agent();
            TIMEE.Model = Settings.GetModelConnection();

            //Add the tool
            Tool generate_timesheet = new Tool();
            generate_timesheet.Name = "generate_timesheet";
            generate_timesheet.Description = "Generate a timesheet and display it to the user for approval.";
            generate_timesheet.Parameters.Add(new ToolInputParameter("description", "A summary description of the timesheet with all necessary details."));
            TIMEE.Tools.Add(generate_timesheet);

            //Add system prompt
            TIMEE.Messages.Add(new Message(Role.system, System.IO.File.ReadAllText(@"C:\Users\timh\OneDrive - Microsoft\Stretch Projects\AI POC Teasers (CHASE IRIS)\TIMEE\prompts\agent1.md")));


        }

    }
}