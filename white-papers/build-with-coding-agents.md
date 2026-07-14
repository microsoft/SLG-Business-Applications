# A Developer's Guide to Building on Power Platform with Coding Agents
Coding agents are quickly becoming a normal part of how developers build software. Coding Agents like GitHub Copilot, Claude Code, and Codex can scaffold, edit, test, and deploy meaningful pieces of an application when they have the right instructions and access.

Power Platform is increasingly becoming an AI-first development platform. Many of its components can now be created, modified, and deployed through coding agents, especially when paired with the right plugins, CLIs, and source-controlled project structures.

This whitepaper helps answer **which Power Platform components are ready for agentic development today, and how should a developer approach them?**

## Can a coding agent make that?
The following table pairs a Power Platform component its current support for creation via Coding Agents and what tools/approaches to use.

| Component | Can a coding agent make it? |
| - | - |
| Solutions | **Yes.** Via [`pac solution`](https://learn.microsoft.com/en-us/power-platform/developer/cli/reference/solution) of the PAC CLI. |
| Power Apps Code Apps | **Yes.** Officially supported via the [Code Apps Plugin](https://github.com/microsoft/power-platform-skills/tree/main/plugins/code-apps). |
| Power Apps Canvas Apps | **Yes.** Officially supported via the [Canvas Apps Plugin](https://github.com/microsoft/power-platform-skills/tree/main/plugins/canvas-apps). |
| Power Apps Model-Driven Apps | **Yes.** Officially supported via the [Model-Driven Apps Plugin](https://github.com/microsoft/power-platform-skills/tree/main/plugins/model-apps). |
| Power Apps Mobile (Native) Apps | **Yes.** Officially supported via the [Mobile Apps Plugin](https://github.com/microsoft/power-platform-skills/tree/main/plugins/mobile-apps). |
| Power Pages Sites | **Yes.** Officially supported via the [Power Pages Plugin](https://github.com/microsoft/power-platform-skills/tree/main/plugins/power-pages). |
| Dataverse Tables & Data | **Yes.** Officially supported via the [Dataverse Plugin](https://github.com/microsoft/Dataverse-skills/tree/main/.github/plugins/dataverse). |
| Power BI Report Authoring | **Yes.** Officially supported via the [Power BI Report Authoring Plugin](https://github.com/microsoft/skills-for-fabric/tree/main/plugins/powerbi-authoring). |
| Custom Connectors & MCP Servers | **Yes.** A coding agent can work against the `connectors` Dataverse table, including OpenAPI definitions stored in connector records. |
| Power Automate Cloud Flows | **Yes.** A coding agent can work against the `workflows` Dataverse table, where cloud flows are represented as Azure Logic Apps JSON. |
| Copilot Studio Agents | **Yes.** Use `pac copilot create`, then edit the generated YAML locally. Agents can also be modified through the `bots` Dataverse record. Experimental [Copilot Studio Plugin](https://github.com/microsoft/copilot-studio-plugin) available too. |
| AI Builder Models & Prompts | **Kind of.** `pac solution export/import` can clone an existing asset, modify it, and re-import it. Useful, but still somewhat obscure. |

## Power Platform Plugins: High-Level Overview
Microsoft officially supports Power Platform development by AI Coding Agents via the [Power Platform Agent Skill Plugins](https://github.com/microsoft/power-platform-skills/). Each plugin contains unique **skills** and **agents** that assist in creating various types of Power Platform resources. When using these agents for development, focus on triggering these skills as you work through the creation lifecycle of the component.

- App Development
    - [Code Apps](https://github.com/microsoft/power-platform-skills/tree/main/plugins/code-apps)
        - `create-code-app`: the main (big) one. Scaffolds app, develops, adds data, implements.
        - `add-datasource`: a *router skill*. Tell it what to do, it picks the right connector-specific skill (e.g. `add-sharepoint` or `add-excel`)
        - `add-connector`: fallback for adding any connector that doesn't have its own skill
        - `deploy`: builds and pushes to Power Platform
        - `list-connections`: gets connection IDs
    - [Canvas Apps](https://github.com/microsoft/power-platform-skills/tree/main/plugins/canvas-apps)
        - `configure-canvas-mcp`: ensures .NET 10 SDK is installed, runs [this MCP server](https://www.nuget.org/packages/Microsoft.PowerApps.CanvasAuthoring.McpServer/1.0.3475.65-preview) using `dnx`
        - `canvas-app`: Creates local folder, pulls current app in (local YAML), builds out screen-builder agent one by one, compiles
    - [Model-Driven Apps](https://github.com/microsoft/power-platform-skills/tree/main/plugins/model-apps)
        - `genpage`: the major skill, basically the whole plugin. Plans, creates tables, creates Model-Driven App.
    - [Mobile Apps (React Native) Apps](https://github.com/microsoft/power-platform-skills/tree/main/plugins/mobile-apps)
        - `create-mobile-app`: orchestrates entire workflow for building Power Apps mobile app with React Native and Expo.
        - `edit-app`: iterates (edits) an existing app accoriding to desired changes.
        - `add-dataverse`: connects to existing Dataverse tables or makes new ones, generating necessary TypeScript services.
        - `add-connector`: integrates with PowerPlatform connectors
        - `list-connectors`: helps find connectors available for use
        - `add-native`: adds support native mobile capabilities like camera, barcode scanner, document picker, file storage access, etc.
        - `design-system`: designs app according to branding standards, design spec, etc.
        - `deploy`: builds and pushes to Power Apps
- Site Development
    - [Power Pages](https://github.com/microsoft/power-platform-skills/tree/main/plugins/power-pages)
        - Site Creation skills
            - `create-site`: scaffold a complete code site, build pages.
            - `deploy-site`: builds project and uploads to Power Pages using `pac pages upload-code-site`. Handles common blockers like JS attachments.
            - `activate-site`: makes site public by provisioning website record.
            - `test-site`: uses Playwright to test the site, crawl it for discovery, verify pages, etc.
            - `setup-datamodel`: creates Dataverse tables w/ columns + relationships by making API calls to Dataverse
            - `add-sample-data`: populates Dataverse tables with data.
            - `integrate-backend`: recommends how to automate what you want, i.e. cloud flow, web API, etc. Will route to `integrate-webapi`, `add-server-logic`, or `add-cloud-flow`
            - `integrate-webapi`: generates code for performing CRUD against Dataverse tables, configures table permissions + site settings so the APIs work in prod.
            - `add-ai-webapi`: integrate4s **Search Summary API** (`/_api/search/v1.0/summary`) and **Data Smmarization API** (`/_api/summarization/data/v1.0/...`).
            - `add-server-logic`: creates **server-side JavaScript** that run securely on Power Pages in Cloud.
            - `add-cloud-flow`: discovers Power Automate flows in the environment, helps with integrating one of those into Power Pages w/ web roles + client-side code.
        - Security skills
            - `create-webroles`: creates web roles (security roles) for site
            - `setup-auth`: adds login/logout functionality and role-based authorization to the site.
- Agents
    - [Copilot Studio](https://github.com/microsoft/copilot-studio-plugin) - *experimental plugin with agents, not skills*
- Data
    - [Dataverse](https://github.com/microsoft/Dataverse-skills/tree/main/.github/plugins/dataverse)
        - `dv-connect`: installs the [Dataverse CLI](https://www.npmjs.com/package/@microsoft/dataverse), Python SDK, PAC CLI. Then authenticates, sets up Dataverse MCP server.
        - `dv-query`: expert at querying Dataverse for you in natural language, like "show me my open deals".
        - `dv-data`: single record CRUD and other data manipulation.
        - `dv-metadata`: For configuring Dataverse tables, columns, relationships, forms, views.
        - `dv-solution`: managed solution lifecycle (export, import, etc.)
        - `dv-admin`: environment administration.
        - `dv-security`: managing security roles, user access, etc.
        - `dv-overview`: intake for all Dataverse needs, routes requests to right specialist skill.
- Misc
    - [MCP Apps Widget Generator](https://github.com/microsoft/power-platform-skills/tree/main/plugins/mcp-apps) - *for creating visual "widgets" that display the results of MCP tool calls within chat interfaces like M365 Copilot. Not sure why it is in the Power Platform repo.*
- [Power BI Report Authoring](https://github.com/microsoft/skills-for-fabric/tree/main/plugins/powerbi-authoring)

## Other capabilities to be aware of
- `pac copilot` command of the PAC CLI allows for lifecycle management of Copilot Studio agents. Docs [here](https://learn.microsoft.com/en-us/power-platform/developer/cli/reference/copilot).
- [Power Apps Code Apps CLI on npm](https://www.npmjs.com/package/@microsoft/power-apps?activeTab=readme) (CLI add-on to the npm SDK)
    - Supports what was once the `pac code` commands from the PAC CLI (see [here](https://learn.microsoft.com/en-us/power-platform/developer/cli/reference/code))
- [Dataverse CLI on npm](https://www.npmjs.com/package/@microsoft/dataverse) - see "Command Reference" section [here](https://www.npmjs.com/package/@microsoft/dataverse)
    - [Docs](https://learn.microsoft.com/en-us/power-platform/developer/cli/introduction?tabs=windows)
    - To run it: `npx dataverse` (downloads, caches, and runs)