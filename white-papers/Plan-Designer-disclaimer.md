
# Can I Be Confident Enabling Plan Designer in GCC?

Plan Designer is an impressive capability that empowers makers to move from idea to first solution iteration in minutes. It accelerates analysis, requirements gathering, front‑ and back‑end development, and documentation — all through natural language.

But if you're in GCC, you’ve probably noticed a pretty intimidating disclaimer when enabling it.

This document explains why the disclaimer exists and provides the context administrators need to make an informed decision.

---

## 📘 Background

### What Plan Designer *is*
Plan Designer is a **development tool**.  
It does **not** access or manipulate your data beyond the text prompts you provide while designing a solution.

### Understanding the compliance boundaries
To make sense of the disclaimer, it’s helpful to understand the architectural context and the perspective from which it was written:

- **Power Platform GCC runs on Azure Government.**  
  When the disclaimer refers to the “relevant Power Platform US Government (GCC, GCC High, and DoD) compliance boundary” it's referring to Azure Government.

- **Microsoft 365 (Office 365) GCC has a different boundary.**  
  Microsoft 365 GCC runs inside a "protected enclave" within Azure Commercial — *not* Azure Government.

Since Plan Designer uses services from both ecosystems, these boundaries matter.

**Power Platform GCC compliance boundary = Azure Government**  
**Microsoft 365 GCC compliance boundary = Protected enclave in Azure Commercial**

Architecture reference:  
![GCC architecture](https://github.com/microsoft/Federal-Business-Applications/blob/main/whitepapers/power-plat-d365-architecture/files/Slide2.PNG)

_Image credit: Federal Business Applications team_

---

## 🧩 What’s Happening with Plan Designer in GCC?

Quite a bit. Plan Designer relies on:

- Power Platform services (Azure Government)
- Azure OpenAI and related Azure services (Azure Government)
- Several Microsoft 365 GCC services (Azure Commercial protected enclave)

All Azure components stay within **Azure Government**, and all Microsoft 365 components stay within **Microsoft 365 GCC**.

Approximate architecture:  
![Plan diesigner GCC architecture](https://imgur.com/BOjZzkI.png)

---

## ⚠️ So Why the Scary Disclaimer?

Because enabling Plan Designer in GCC triggers two compliance considerations:

### **1. Use of services *outside* the Power Platform (Azure Gov) compliance boundary**
Microsoft 365 GCC services aren’t part of the Power Platform GCC boundary.  
Any communication crossing that line — even between government‑compliant systems — must be explicitly disclosed.

### **2. A FedRAMP High nuance around Azure Fluid Relay’s usage**
Plan Designer’s collaboration functionality leverages components abstracted from **Azure Fluid Relay**.

- Azure Fluid Relay *is* FedRAMP High authorized.  
- However, the *integration path* between Fluid Relay and Power Platform has not yet completed its FedRAMP High audit.  
- Because of that nuance, Microsoft must state that not all resources used by Plan Designer are FedRAMP High authorized.

The disclaimer is disclosure-driven; it is not an indication of perceived risk.

---

## ✅ Key Facts to Inform Your Decision

1. **Plan Designer only assists solution development.**  
   It doesn't interact with your organizational data beyond user-entered prompts and has no impact on runtime behavior.

2. **All Plan Designer components remain inside Azure Government or Microsoft 365 GCC.**  
   Nothing leaves these boundaries.

3. **Tenant control is granular.**  
   Even after enabling it at the tenant level, admins decide which environments — and therefore which makers — can use Plan Designer.

---

## 🛠️ How to Enable Plan Designer

### **Who can enable it?**
- Global Administrators  
- Power Platform Administrators  

### **Where to enable it?**
In the **Power Platform Admin Center**, at both the *tenant* and *environment* levels.

---

### 🔧 Tenant-Level Setting

**Path:**  
`Manage → Tenant Settings → Plan Designer for solution development`

If you don’t see *Tenant Settings*, you do not have tenant-level permissions.

![Tenant level setting](https://imgur.com/tcxFaot.png)

---

### 🔧 Environment-Level Setting

**Path:**  
`Manage → Environments → <Environment> → Generative AI settings → Microsoft 365 services`

If you lack tenant-level permissions, saving will produce an error.

![Environment level setting](https://imgur.com/V5tVQop.png)

---

## 🚀 Using Plan Designer

Once enabled at both levels, Plan Designer appears on the Power Apps home page.

You can validate functionality by selecting a suggested prompt and generating a plan.

![Enter a prompt](https://imgur.com/aeVtK5v.png)

![Work the plan](https://imgur.com/atIZEGw.png)

For a longer walkthrough, check out the [Tech Series demo (Oct 2025)](https://www.youtube.com/watch?v=_QqvWMLqbYc).

---

## 👨‍💻 Author

- [Doug Bell](https://www.linkedin.com/in/doug-bell-56090341/), *Power Platform Senior Solution Engineer*
