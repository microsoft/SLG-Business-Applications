using System;
using TimHanewich.AgentFramework;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TIMEECore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(LastWeekDescriptor(DateTime.Today));
        }

        public static string LastWeekDescriptor(DateTime today)
        {
            //Prints all dates of last week so TIMEE knows what dates are which
            int daysSinceMonday = ((int)today.DayOfWeek + 6) % 7; // normalize so Monday = 0
            DateTime lastMonday = today.AddDays(-daysSinceMonday - 7); // jump back to last Monday

            List<DateTime> lastWeekDays = Enumerable.Range(0, 5).Select(offset => lastMonday.AddDays(offset)).ToList();

            if (lastWeekDays.Count == 5)
            {
                return "Monday was " + lastWeekDays[0].ToShortDateString() + "\nTuesday was " + lastWeekDays[1].ToShortDateString() + "\nWednesday was " + lastWeekDays[2].ToShortDateString() + "\nThursday was " + lastWeekDays[3].ToShortDateString() + "\nFriday was " + lastWeekDays[4].ToShortDateString();
            }
            else
            {
                return "idk";
            }
        }

        public static async Task<JObject> GenerateTimesheetAsync(string description)
        {
            Agent a = new Agent();
            a.Model = Settings.GetModelConnection();

            //Construct system prompt (add last week's dates to it)
            string SYSTEM = Prompts.TimesheetGeneratorSystemPrompt;
            SYSTEM = SYSTEM + "\n\n" + "Last week's dates:" + "\n" + LastWeekDescriptor(DateTime.Today);

            //Add system prompt
            a.Messages.Add(new Message(Role.system, SYSTEM));

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