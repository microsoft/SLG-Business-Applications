# Copilot Studio Knowledge Manager

![Power Platform](https://img.shields.io/badge/Power%20Platform-Copilot%20Studio-blue)  
*A no-code/low-code solution for delegated knowledge source management in Copilot Studio.*

---

## 👥 Who is this for?

This solution is designed for:
- **IT administrators** who manage Copilot Studio agents
- **Business users or analysts** who maintain knowledge sources
- **Power Platform makers** looking to extend Copilot Studio functionality

---

## 📚 Table of Contents

- [What Problem Are We Solving?](#what-problem-are-we-solving)
- [Is There Another Option?](#is-there-another-option)
- [Our Solution](#our-solution)
- [How to Deploy](#how-to-deploy)
- [Limitations](#limitations)
- [Security Roles and Permissions](#security-roles-and-permissions)
- [Other Solution Components](#other-solution-components)
- [Extending This Solution](#extending-this-solution)
- [Development Team](#development-team)

---

## ❓ What Problem Are We Solving?

Copilot Studio is a powerful tool for building Generative AI agents that reason over your data. However, **permissions are often all-or-nothing**, meaning users either have full control over an agent or none at all.

We've heard from several state and local government entities that they need a **delegated authority model**—specifically, the ability to allow users to manage **file-based knowledge sources only**, without granting full agent control.

---

## 🔄 Is There Another Option?

You might ask: *Why not use SharePoint as a knowledge source and delegate content management there?*

While valid, this approach has two key limitations:
- **Indexing and search quality**: Files uploaded directly to Copilot Studio are indexed and searched by Dataverse, which may outperform SharePoint.
- **Authentication**: SharePoint requires Entra ID, making it unsuitable for public-facing agents.

---

## 💡 Our Solution

We built a Power Platform-based solution that acts as an **admin console** for managing file-based knowledge sources in Copilot Studio.

### Key Features:
- Select an agent
- View existing knowledge files
- Add or remove files
- Reflect changes in Copilot Studio

This is enabled through a Power App and supporting components included in the [unmanaged solution file](https://github.com/microsoft/SLG-Business-Applications/releases/download/41/CopilotStudioknowledgemanager_1_0_0_2.zip).

### 🎥 Demo Video

[![demo video](https://i.imgur.com/0SNuRNJ.png)](https://youtu.be/bEMVhDxTenE)  
*If the video above does not work, click [here](https://youtu.be/bEMVhDxTenE).*

---

## 🚀 How to Deploy

1. Download the [unmanaged solution file](https://github.com/microsoft/SLG-Business-Applications/releases/download/41/CopilotStudioknowledgemanager_1_0_0_2.zip).
2. Open Power Apps and navigate to your **non-production environment**.
3. Import the solution.
4. Assign the required security roles (see below).
5. Launch the app and start managing knowledge sources.

---

## ⚠️ Limitations

- Only **file-based knowledge sources** are supported.
- Other knowledge types (e.g., websites, SharePoint) are not included.
- The app does **not publish agents**—this must be done manually in Copilot Studio. In the app, new files are tagged as **Draft** until the agent is published.

---

## 🔐 Security Roles and Permissions

To use this app, users need:
- **Solution Customizer** (standard role)
- **Copilot Studio Knowledge Manager** (custom role included in the solution)

> Note: Copilot Studio components are "Solution Components" in Dataverse, requiring elevated permissions to modify (hence the need for the Solution Cusomizer role).

---

## 🧩 Other Solution Components

The solution also includes a **Power Automate flow** that sends a Teams notification to the agent owner whenever a new file is added.

---

## 🛠️ Extending This Solution

This is an **unmanaged solution**, so you're free to customize it!

Ideas for extension:
- Add a **Publish** function (see: [How To Add Copilot Studio Knowledge Files Using Power Automate](https://www.matthewdevaney.com/how-to-add-copilot-studio-knowledge-files-using-power-automate/#Publish-The-Copilot-Studio-Agent-Using-A-Bound-Action))
- Build a **review/approval workflow**
- Integrate with other Power Platform tools

Let your creativity guide you—Power Platform is your playground!

---

## 👨‍💻 Development Team

Built by the U.S. State & Local Government team at Microsoft:

- [Doug Bell](https://www.linkedin.com/in/doug-bell-56090341/), *Power Platform Technical Specialist* – Design and Implementation  
- [Kelly Cason](https://www.linkedin.com/in/kellycason/), *Business Applications Technical Specialist* – UI & UX Design  
- Special thanks to Doug Furney and John Parker for their consulting support.
- With inspiration and guidance from the writings of [Matthew Devaney](https://www.matthewdevaney.com/about/)

