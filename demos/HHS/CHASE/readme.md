<p align="center">
    <img src="https://i.imgur.com/1XdPBGa.png" alt="CHASE banner" width="60%">
</p>

# CHASE: Child Health and Safety Expert
**CHASE**, short for **Child Health And Safety Expert**, is a conceptual AI agent for child welfare case workers. CHASE was designed with the goal of giving case workers more time for the human work that matters most. AI agents can help carry the repetitive administrative load, connect information across systems, and turn next steps into momentum, while the case worker remains in control.

The demo below follows Sarah Mitchell, a child welfare case worker, as she supports the Williams family through a stressful period involving job loss, food insecurity, school lunch support, and follow-up services. CHASE acts as Sarah's AI sidekick inside a child welfare workflow, helping her prepare for visits, capture case context, identify risks, draft follow-ups, and move administrative work forward while Sarah stays focused on the family.

[![demo_video_thumbnail](https://i.imgur.com/kZder4h.png)](https://youtu.be/F4pPeF-AMxQ)

## What technology makes this possible?
CHASE is a vision for how AI agents can modernize child welfare. It brings together Microsoft technologies into a connected solution that supports both frontline case workers and the families they serve.

![solution architecture](https://i.imgur.com/WZIj2np.jpeg)

- **Copilot Studio** powers the CHASE agent experience. An autonomous background agent reviews prior case history to generate Sarah's pre-visit briefing, then analyzes visit transcripts to draft case notes, surface risks, recommend next steps, and begin approved follow-up work in parallel.
- **Power Apps** provides the CHASE experience for case workers: a single place for Sarah to review dashboards, visit summaries, recommendations, and the progress CHASE is making on her behalf.
- **Power Pages** hosts the public-facing SNAP portal, giving families like Rebecca's a simple digital experience to resume, complete, and submit benefit applications.
- **Dynamics 365** and **Dataverse** provide the unified HHS platform foundation, connecting child welfare and eligibility processes across a shared data and application layer:
    - **Comprehensive Child Welfare Information System (CCWIS)**: Built on Dynamics 365, the CCWIS system serves as the system of record for family case history. CHASE reads from it to understand case context and writes back draft notes, suggested actions, and task progress for worker review.
    - **Integrated Eligibility System (IES)**: Built on Dynamics 365, the IES system receives and stores SNAP applications submitted through the online portal. Because it operates on the same platform foundation as CCWIS, CHASE can intelligently pre-start an application using information already collected through the child welfare process.

## Credits
CHASE was created by [Tim Hanewich](https://github.com/TimHanewich).

> Note: If you are looking for the older version of CHASE (2023), click [here](https://github.com/microsoft/SLG-Business-Applications/tree/511001efea72c840784fbe21cdb1d5f66027f697/demos/HHS/CHASE).