# 🏞️ Public Park Maintenance 🛠️
This solution is designed to streamline the management, reporting, and execution of public park maintenance. By leveraging the Microsoft Power Platform, this system bridges the gap between public reporting and internal maintenance operations to resolve issues.

*Click the image below for a short demo of the finished solution!*

[![thumbnail](https://i.imgur.com/OCXpBom.png)](https://youtu.be/ptLcezYuq8c)

## Value for State & Local Government
Why is this Park Maintenance solution useful for SLG organizations?
- ⚡ **Increased Responsiveness**: Reduces the time between a citizen spotting an issue and a technician receiving a work order.
- 📊 **Data-Driven Budgeting**: Dataverse stores historical maintenance records, helping governments identify which parks need more funding or equipment upgrades.
- 🤝 **Accessibility**: Using an AI Agent (Copilot) means citizens don't have to navigate complex government websites to file a simple report.

## System Architecture
![architecture](https://i.imgur.com/UwXEbzU.png)
- **Dataverse** serves as a unified data backbone, ensuring data integrity across all main solution touchpoints.
- **Copilot Studio** is used for an AI agent allowing citizens to report issues (i.e. broken swings, fallen trees) using natural language.
- **Power Apps** is used for a comprehensive back-office app for internal staff to triage, assign, and track the lifecycle of maintenance tasks.
- **Power Apps** is used for a mobile-optimized app for maintenance crews to view their daily queue and mark tasks as complete in real-time.
- **Power Automate** serves as the automation layer that sends instant email notifications to staff when new maintenance tasks are assigned to them.

## Data Model
![data model](https://i.imgur.com/ETwvsnX.png)
A robust data model, built on Microsoft Dataverse, is used in this solution:
- `Park`: Acts as the primary "Parent" entity, storing location metadata and identifying the internal manager responsible for the site.
- `Park Issue`: The central hub of the data model. This table captures the raw report from the public via Copilot Studio, including descriptions and photographic evidence.
- `Maintenance Task`: A child entity linked to specific issues. This allows managers to break down a single reported issue (e.g., "Park Vandalism") into multiple actionable work orders for field staff.

## Assets
- [The completed solution file (.zip)](https://github.com/microsoft/SLG-Business-Applications/releases/download/48/ParkMaintenance_1_0_0_1.zip)
- [Park Backdrop Image](https://github.com/microsoft/SLG-Business-Applications/releases/download/48/park.jpg), to be used as background for Canvas App
- [Park icon](https://github.com/microsoft/SLG-Business-Applications/releases/download/48/park-icon.png), to be used as icon for Copilot Studio agent

## Credits
Developed by [Tim Hanewich](https://github.com/TimHanewich), *Sr. AI Business Process Solution Engineer at Microsoft*.