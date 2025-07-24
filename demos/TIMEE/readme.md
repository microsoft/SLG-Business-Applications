# 🕒 TIMEE: **Time**sheet **E**ntry Agent
![banner](https://i.imgur.com/wXgrfTF.jpeg)

**TIMEE**, pronouned *Timmy*, is a conversational agent that transforms how organizations handle timesheet reporting. Instead of wrestling with rigid, manual entry systems, staff can simply talk to TIMEE - an intuitive AI chatbot built to understand natural language descriptions of work activities.

Just describe your week, and TIMEE takes care of the rest: interpreting your input, auto-filling the timesheet, and allowing further refinement through follow-up chat. Submit with confidence and skip the hassle.

TIMEE is powered by Microsoft Power Platform and Azure AI Foundry, leveraging scalable cloud technologies and advanced AI capabilities to streamline operational workflows and enhance user experience.

Check out a demo of TIMEE by clicking the image below!

[![timee](https://i.imgur.com/txoBBdI.png)](https://youtu.be/aq7pQY8-Qe0)

## How does TIMEE Work?
TIMEE uses an advanced multi-agent architecture to achieve the functionality demonstrated. 

The first agent is **TIMEE**. This is the AI agent that communicates with the user. TIMEE is designed to be conversational, helpful, and polite in its tone. TIMEE's main job is to collect relevant information about the user's work week that will eventually be entered into a timesheet in a structured format. Upon conversing with the user and collecting all relevant information, TIMEE passes this information along to the next agent.

The second agent is the **Timesheet Generator Agent**. This agent is *not* directly communicated with by the user and, in fact, is completely invisible to the user. TIMEE acts as a broker between this agent and the user, collecting relevant information from the user about their work week and in turn providing that information to the Timesheet Generator Agent. The Timesheet Generator Agent is *not* a conversational agent - instead, it serves one purpose: generate timesheets in a standard, structured format.

**TIMEE** collects relevant information from the user and passes along an unstructured summary of that information to the **Timesheet Generator Agent** once sufficient information to make a timesheet is collected.  The Timesheet Generator Agent takes the summary and converts it to a standard structured format that is understood and agreed upon between the two agents. As the conversation between the user and TIMEE progresses, TIMEE then further communicates any relevant changes to the Timesheet Generator Agent, which then produced the new timesheet and provides it to TIMEE, when it is then shown to the user.

## API System
Because that multi-agent architecture described above can be complicated to facilitate within-app, an API layer exists to *abstract* that complexity away from the app layer. Moreover, this API layer is the code layer that facilitates the back and forth communication between **TIMEE** and the **Timesheet Generator Agent**. To both *the user* and *the developer of the app*, that background process is invisible; all the user and developer see are messages being submitted to TIMEE and a response message and timesheet coming out!

The API layer has two inputs that must be provided in an API call to TIMEE:
- **SessionKey** (plain text) = a random string that uniquely identifies the chat "session". Chat history is stored *sever-side*, so the TIMEE API uses this key to know what message history to retrieve and load into memory before generating an AI response and timesheet.
- **User Message** (plain text) = the new user inquiry (i.e. further clarification about the timesheet, any tweaks or adjustments, etc.)

*Note, the SessionKey parameter above can be any random string - you make it up! It is just a way to formally load and save chat history*

The API layer provides two distinct outputs, both of which are visible to the user:
- **Proposed Timesheet** (large JSON object) = the Timesheet Generator Agent's generated timesheet in a structured format
- **Response Message** (plain text) = the plain text response from TIMEE (i.e. a confirmation of a change, further commentary, or a follow-up question)

The Following is an example body for an HTTP POST call to the TIMEE API:
```
{
    "key": "d101a9c056ab43b5a162ac3084f555aa",
    "message": "I worked every day last week from 7 AM to 3 PM with 60 minute lunches, but I took Wednesday off for parental leave."
}
```

And the following is an example of the response TIMEE would provide:
```
{
    "message": "Your timesheet has been updated:\n\n- Worked 7:00 AM – 3:00 PM with a 1-hour lunch on Monday, Tuesday, Thursday, and Friday\n- Took Wednesday (7/16) off using 8 hours of parental leave\n\nIf everything looks good, you’re all set! If anything else needs adjusting, just let me know.",
    "timesheet": {
        "monday": {
            "date": "2025-07-14",
            "in": 7,
            "out": 15,
            "lunch": 1,
            "leaveCode": null,
            "leaveHours": 0,
            "leaveReason": null
        },
        "tuesday": {
            "date": "2025-07-15",
            "in": 7,
            "out": 15,
            "lunch": 1,
            "leaveCode": null,
            "leaveHours": 0,
            "leaveReason": null
        },
        "wednesday": {
            "date": "2025-07-16",
            "in": 0,
            "out": 0,
            "lunch": 0,
            "leaveCode": "P",
            "leaveHours": 8,
            "leaveReason": "Parental leave"
        },
        "thursday": {
            "date": "2025-07-17",
            "in": 7,
            "out": 15,
            "lunch": 1,
            "leaveCode": null,
            "leaveHours": 0,
            "leaveReason": null
        },
        "friday": {
            "date": "2025-07-18",
            "in": 7,
            "out": 15,
            "lunch": 1,
            "leaveCode": null,
            "leaveHours": 0,
            "leaveReason": null
        }
    }
}
```

## Architecture
*(complete architecture diagram coming soon)*

## Solution
*(solution file and source code coming soon)*

## Media Assets
- TIMEE Avatars
  - [Main](https://i.imgur.com/kVoZQlJ.png)
    - [Flipped horizontally](https://i.imgur.com/Db1RQUy.png)
  - [Thinking](https://i.imgur.com/QTOdNxJ.png)
  - [Mailbox](https://i.imgur.com/T1rFNN3.png)
  - [Celebrating](https://i.imgur.com/0adH6TQ.png)
- Other assets
  - [Loading GIF](https://i.imgur.com/DCePhoO.gif)
  - [Gradient](https://i.imgur.com/dUNT54U.jpeg)