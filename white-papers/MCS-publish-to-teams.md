# Publish Copilot Studio Agents to Teams in GCC

![Power Platform](https://img.shields.io/badge/Power%20Platform-Copilot%20Studio-blue)

Copilot Studio is a fantastic no-code / low-code tool for creating custom chatbot agents armed with Generative AI reasoning over your data. Within minutes, a no-code maker can build a Generative AI agent that'll extract knowledge from specific documents, public websites, SharePoint repositories, and other data sources.

There are numerous ways to distribute agents built in Copilot Studio. For internal-facing agents, what better way to distribute them than via Microsoft Teams?

---

## ❓ What Problem Are We Solving?

While the **Teams channel** has appeared as an available distribution option in the **Government Community Cloud (GCC)** for some time, customers often report being unable to publish their agents to Teams.

This perception is understandable—but ultimately incorrect. This guide clarifies the limitation and provides a working solution for publishing Copilot Studio agents to Teams in GCC.

---

## 🚫 The Limitation

In short, the **"easy button"** option doesn't work in GCC. When users attempt to use the **"See agent in Teams"** button after adding the Teams channel, they encounter a **"Something went wrong"** error.

![Error message after clicking "see agent in Teams"](https://i.imgur.com/JhUwwnH.png) 
> *[Image : Error message after clicking "See agent in Teams"]*

---

## ✅ The Solution

The good news: there is a reliable workaround.

### Step-by-Step:

1. In Copilot Studio, go to the **Teams channel** settings for your agent.
2. Click **"Availability options"**.
3. Select **"Download .zip"** to export a custom Teams app package.

![Availability options → Download .zip](https://i.imgur.com/00JMU9Z.png)
> *[Image: Availability options → Download .zip]*

> ⚠️ Ignore all other options in the dialog—they do not apply in GCC.

---

## 🧭 What to Do with the .zip File

There are two paths forward, but the most common is to hand off the `.zip` file to a **Teams Administrator**.

### Admin Instructions:

- Upload the `.zip` file via the **Teams Admin Center**.
- Configure access using **Teams App Permission Policies**.

📚 Helpful Docs:
- [Manage custom app policies and settings - Microsoft Teams](https://learn.microsoft.com/microsoftteams/teams-custom-app-policies-and-settings)
- [Manage app permission policies in Microsoft Teams](https://learn.microsoft.com/microsoftteams/teams-app-permission-policies)

Once uploaded and scoped, authorized users can find and install the agent from the **Teams App Store**.
![Download agent from Teams App Store](https://i.imgur.com/bEPqWHQ.png)
> *[Image: Download agent from Teams App Store]*

---

## 🧩 A Note About the Custom App

The downloaded `.zip` file contains:
- The agent name and icon
- A pointer to the agent hosted in Copilot Studio

It does **not** contain any logic or knowledge content. All behavior remains in Copilot Studio.

### 🔄 What This Means:
When makers publish updates to the agent in Copilot Studio, **no changes are needed in Teams**. The Teams app is just a pointer—updates are reflected automatically within minutes.

---

## 📌 Summary

| Feature | GCC Support |
|--------|--------------|
| "Easy button" publich to Teams | ❌ Not supported |
| Download .zip and upload via Teams Admin | ✅ Supported |
| Auto-reflect agent updates in Teams | ✅ Supported |
| Other availability options | ❌ Not applicable base on the method we're using |

---

## 👨‍💻 Author

- [Doug Bell](https://www.linkedin.com/in/doug-bell-56090341/), *Power Platform Technical Specialist*
