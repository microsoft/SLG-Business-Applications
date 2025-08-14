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
3. On the following screen you have two options that work:
    1. **Show to everyone in my org** to route the agent for approval in the Teams Admin Center (preferred for admin approval)
    2. **"Download .zip"** to export a custom Teams app package

<img width="1110" height="420" alt="Availability options ➡️ Settings" src="https://github.com/user-attachments/assets/8e756d06-f614-49b4-8e68-8b1613b25f01" />

> *[Image: Availability options → Download .zip]*

> [!WARNING]  
> ⚠️ Ignore all other options in the dialog — they do not work in GCC.

---
## 👍 Admin approval for "Show to everyone in my org" option

From the agent creator's perspective, you continue through the dialog to send the request to a Teams admin
<img width="2046" height="717" alt="Show to everyone in my org - creator UX" src="https://github.com/user-attachments/assets/6e13bf39-3b9d-4d1e-a26d-f7b1c649c9da" />

> *[Image : Visual step-by-step process for agent creator requesting to publish an agent to Teams"]*

From the Teams admin perspective, once the request has been submitted, the app will show with a **Pending Action** notice in the admin center. The admin can then publish or reject the request.
<img width="1242" height="716" alt="Show to everyone in my org - admin UX" src="https://github.com/user-attachments/assets/1925061c-221c-4a5d-a890-340a24bc8d69" />

> *[Image : Visual step-by-step process for admin approving publishing an agent to Teams"]*

> [!NOTE]  
> The Teams admin will not automatically be notified that an agent's app package is pending their action. Your organization should determine a process to request admin support for this action.

---

## 🧭 What to Do with the .zip File

There are two paths forward, but the most common is to hand off the `.zip` file to a **Teams Administrator**.

> [!WARNING]  
> ⚠️ The other option for the `.zip` file is for individual users to "side-load" the agent's app package `.zip` file into Teams for their personal use (see [Upload your custom app - Teams | Microsoft Learn](https://learn.microsoft.com/en-us/microsoftteams/platform/concepts/deploy-and-publish/apps-upload)). Uploading custom apps in this way is possible only in Government Community Cloud (GCC) and isn't possible in GCC High, Department of Defense (DoD).
>
> To disable this functionality and require teams apps go through admin approval, turn off Custom apps in Org-wide app settings in the Teams Admin Center (see [Manage custom app policies and settings - Microsoft Teams | Microsoft Learn](https://learn.microsoft.com/en-us/microsoftteams/teams-custom-app-policies-and-settings#allow-users-to-upload-custom-apps)).

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
| "Easy button" publish to Teams | ❌ Not supported |
| Publish using "Show to everyone in my org" option + approval from Teams Admin  | ✅ Supported |
| Download .zip and upload via Teams Admin | ✅ Supported |
| Auto-reflect agent updates in Teams | ✅ Supported |
| Other availability options | ❌ Not applicable base on the method we're using |

---

## 👨‍💻 Authors

- [Doug Bell](https://www.linkedin.com/in/doug-bell-56090341/), *Power Platform Senior Solution Engineer*
- [Grayson Bishop](https://www.linkedin.com/in/grayson-bishop), *Power Platform Senior Solution Engineer*
