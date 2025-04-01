# TIPLI: Tip & Lead Intake via Copilot Studio Autonomous Actions
**TIPLI**, short for **Tip** and **L**ead **I**ntake, demonstrates how Copilot Studio's autonomus actions capability can be used to build an intelligent **agent** that possesses the capability of conversing with visitors of a public-facing website, collecting details about tips and leads that a member of the public witnessed, matching and saving it against an existing investigative case, and scheduling a follow up call with an investigator.

Click the image below for a demo of TIPLI:

[![TIPLI demo](https://i.imgur.com/bkn56vM.png)](https://youtu.be/z85WHeHtLQc)

## How does TIPLI work?
Imagine you are hiring a human for the task of staffing a tip reporting phone line and are in the early onboarding/training phases, preparing them for success in their role; what would they need to do their job successfully? 

You would provide them with instructions or best practices on how to carry out their job using standard operating procedures. You'd likely give them access to a backend CRM-like system where they can do things like find cases, read cases, and save new leads to cases. You may want to give them access to the calendar of a detective or investigator team to schedule follow up calls with callers as necessary.

Building an autonomous AI agent is **no different**. Much like a human, agents require instructions on how to do their job and access to systems to carry out their work. In Copilot Studio, we call these **Instructions** and **Autonomous Actions**.

TIPLI is a demonstration of how Copilot Studio can be used to build a tip reporting agent (chatbot on your website or phone line) that can collect tips and leads from the public. To build TIPLI, we give a Copilot Studio agent **autonomus actions** and **system instructions** on how to use those autonomous actions to carry out its task.

### Equipping TIPLI with Autonomous Actions (Tools)
*"What are Autonomous Actions?"* you may ask. Also known as *tools* or *functions* (tool calling/function calling), autonomous actions are individual capabilities that the agent can call upon at will as it needs to to carry out its assigned duties. The agent, powered by the intelligence of a large language model, calls upon the actions intelligently as it deems necessary to do so. Think of these like tools the agent has in its back pocket that it can use throughout a conversation to accomplish the task you gave it.

We are giving our Copilot Studio agent **four** "actions" it can call to based upon when it deems it is necessary. 

The actions we are giving TIPLI are as follows:

![TIPLI actions](https://i.imgur.com/W9wCO4T.png)

1. **Find Matching Case**: when conversing with the visitor, TIPLI has the ability to use this action to match what the visitor witnessed with an existing investigative case in our backend system. TIPLI collects and provides a description of the case while a backend workflow finds the correct case and provides this match to TIPLI.
    - The inputs to this action are as follows:
    - **Description**: description of the tip that was provided.
    - **Witnessed At**: the date and time the eyewitness encountered what they are reporting.
2. **Save Lead to Case**: once the visitor has provided enough information, TIPLI can use this action to save the details about what they witnessed to the backend system, correctly associating their tip/lead with the correct case.
    - The inputs to this action are as follows:
    - **Case ID**: the ID of the case to associate this lead with, discovered from previously using the *Find Matching Case* action.
    - **Witness First Name**: first name of the reporting witness.
    - **Witness Last Name**: lirst name of the reporting witness.
    - **Description**: the tip/lead described.
    - **Locaiton**: where the reported incident was reported
    - **Witnessed At**: when the reported incident was observed by the reporter.
3. **Find Availability for Follow Up Call**: after TIPLI successfully saves all the reported tip via the *Save Lead to Case* action, TIPLI then follows its instructions and inquires if the visitor is open to a follow up call with an investigator/detective. If the visitor says *yes*, TIPLI uses the *Find Availability for a Follow Up Call* action to check the detective's outlook calendar for available times and offers these times to the visitor.
4. **Schedule Follow Up Call**: after TIPLI presents the available times and the visitor selects one of the times, TIPLI then uses this action to schedule an appointment on the detective's calendar for this follow-up call.
    - The inputs to this action are as follows:
    - **Selected Date Time**: the date/time that the visitor selected for the follow up call.
    - **Phone Number**: the phone number of the visitor that the detective will call (in XXX-XXX-XXXX format).

Now that TIPLI has these four actions available to use, we can move on to the next stage in TIPLI's development, providing it with explicit instructions on how/when it should use these!

### Instructing TIPLI on how to Operate (System Prompt)
With TIPLI equipped with the four actions ("tools" or "capabilities") described above, now we describe to TIPLI *how it should go about using these to accomplish its task*. In Copilot Studio we do this by providing our agent, TIPLI, with **Instructions**, also known as a *system prompt*.

These instructions specify how TIPLI should behave, conduct itself, and carry out its assigned duties, much like you would also provide to a human! You can make these instructions are simple or elaborate as possible, but prompt engineering best practices usually recommend you be as specific as possible.

Below are the instructions I developed for TIPLI:

```
You are TIPLI, a Tip & Lead Intake agent that serves as an AI-driven call dispatcher at a police department. Members of the public will contact you to report tips and leads on cold cases we are working on.

Here are your step-by-step instructions for working with people that contact you:
1. Collect Information: Work with them to collect detailed information about what they saw. Ask specific questions about the event, including what they saw and when they saw it.
2. Find the case: Provide a summary of this collected information (what they saw) to the "Find Matching Case" action. This action will return the correct case (incident).
3. Confirm the case: Inform the caller that you have found the correct case without divulging too much information. Keep the explanation high level and confirm your understanding with the caller.
4. Collect Further Information: Ask the eyewitness to describe what they saw in further detail. Collect important information that may be used in future invesgitations. Collect information like the date/time they witnessed it, the specific location where it was witnessed, and their first and last name.
5. Log the lead: Use the "Save Lead to Case" action to log detailed information about what the eyewitness saw. Before submitting the details, confirm the accuracy with the caller.
6. Thank them: After saving the lead, inform the caller that their report has been saved and thank them for reporting it.
7. Offer a follow-up call: Politely ask the caller if they are open to a follow-up call with Chief Detective Jose Gomez. If they decline, thank them again and end the conversation. If they accept, proceed to the next step.
8. Find Available Times for Follow-Up Call: Use the "Find Availability for Follow-Up Call" action to search through Detective Gomez's calendar for availability. Provide the available tmes to the caller and ask which time works best for them.
9. Schedule Follow-up Call: Use the "Schedule Follow-Up Call" action to schedule the call on Detective Gomez's calendar. Collect necessary information from the caller, such as their cell phone number in XXX-XXX-XXXX format.
10. Confirm the follow up call: Confirm with them that Chief Detective Jose Gomez will be calling them at the scheduled time and thank them again.
```

These can easily be entered into the **Instructions** in the **Overview** tab of our agent in Copilot Studio.

![TIPLI instructions](https://i.imgur.com/kpa0sFm.png)

## Credit
TIPLI created by [Tim Hanewich](https://github.com/TimHanewich).