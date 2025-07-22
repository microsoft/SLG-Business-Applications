# TIMEE: **Time**sheet **E**ntry Agent

## Two Agents:
- Agent 1 = TIMEE
    - Conversational
    - Role = chat with the user about their timesheet needs.
    - Collect all relevant information
    - Then provide all relevant information to the next agent...
    - Has "**Generate Time Sheet**" tool for calling to time sheet generator
- Agent 2 = Time Sheet Generator
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