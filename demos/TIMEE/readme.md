# TIMEE: **Time**sheet **E**ntry Agent

## Two Agents:
- Agent 1 = TIMEE
    - Conversational
    - Role = chat with the user about their timesheet needs.
    - Collect all relevant information
    - Then provide all relevant information to the next agent...
    - Has "**Generate Time Sheet**" tool for calling to time sheet generator
- Agent 2 = Timesheet Generator Agent
    - Role = **only generate a timesheet from a single input**
    - System prompt = elaborate prompt + structure specified
    - User prompt = description of timesheet needs as TIMEE
    - Outputs timesheet as JSON, outputs it + is handled programmatically
    - Responds "the user is now seeing the timesheet you specified" to TIMEE

## Overall System

### API System
- Inputs:
    - SessionKey (plain string) = key to look up message history, stored in memory on the server
    - New User Message (plain string) = the new user inquiry
- Outputs:
    - Generated (proposed) timesheet, if any (plain string)
    - Response message, if any (plain string)

Example API Call:
```
{
    "key": "d101a9c056ab43b5a162ac3084f555aa",
    "message": "I worked every day last week from 9-5 AM with 30 minute lunches."
}
```

Example API response:
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