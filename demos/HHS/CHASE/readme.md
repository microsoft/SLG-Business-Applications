<p align="center">
    <img src="https://i.imgur.com/1XdPBGa.png" alt="CHASE banner" width="60%">
</p>

# CHASE: Child Health and Safety Expert
**CHASE**, short for **Child Health And Safety Expert**, is a conceptual AI agent for child welfare case workers. CHASE was designed with the goal of giving case workers more time for the human work that matters most. AI agents can help carry the repetitive administrative load, connect information across systems, and turn next steps into momentum, while the case worker remains in control.

The demo below follows Sarah Mitchell, a child welfare case worker, as she supports the Williams family through a stressful period involving job loss, food insecurity, school lunch support, and follow-up services. CHASE acts as Sarah's AI sidekick inside a child welfare workflow, helping her prepare for visits, capture case context, identify risks, draft follow-ups, and move administrative work forward while Sarah stays focused on the family.

[![demo_video_thumbnail](https://i.imgur.com/kZder4h.png)](https://youtu.be/F4pPeF-AMxQ)

## What Technology makes this possible?
CHASE, a vision for the future of Child Welfare in the era of AI Agents, is powered by a group of discreet capabilities on the Microsoft Powerplatform. These technologies come together to create a unified solution that is both external facing (families) and internal facing (Children & Family Service).

![solution architecture](https://i.imgur.com/WZIj2np.jpeg)

- **Copilot Studio** serves as the AI Agent engine, performing in the background in parallel to Sarah's efforts. A Copilot Studio autonomous (background) agent reviews prior case history to synthesize the pre-visit briefing that Sarah reads in the morning. Post-visit, this agent reviews the transcript from the meeting and takes action to proactively file draft notes and suggested next steps for Sarah for her to review later. When Sarah selects which next steps to pursue, it works in the background to get a head start on those tasks.
- **Power Apps** serves as CHASE's User Interface, a pane of glass that Sarah can use to review CHASE's output, including the dashboards, visit summaries, and proactive work against the case.
- **Power Pages** serves to host the public-facing SNAP Portal, allowing families like Rebecca's to visit a public-facing site to complete and submit their application for SNAP benefits.
- **Dynamics 365** and **Dataverse** serve as a Unified HHS Enterprise platform, with two spokes:
    - **Comprehensive Child Welfare Information System (CCWIS)**: a CCWIS system, built on Dyamics 365, serves as the core CCWIS system, securely storing full case history for all families. CHASE integrates with this system to both read data to acquire case context and write data (drafted case notes, suggested next steps, task progress, etc).
    - **Integrated Eligibility System (IES)**: a IES system, built on Dynamics 365 serves as the core IES system, securely receiving and storing SNAP applications that are submitted via the online portal. Because this IES system is built on the same technology as their CCWIS system (a unified platform), CHASE is able to begin a SNAP application leveraging family information previously collected as part of the Child Welfare process.

## Credits
CHASE was created by [Tim Hanewich](https://github.com/TimHanewich).

> Note: If you are looking for the older version of CHASE (2023), click [here](https://github.com/microsoft/SLG-Business-Applications/tree/511001efea72c840784fbe21cdb1d5f66027f697/demos/HHS/CHASE).