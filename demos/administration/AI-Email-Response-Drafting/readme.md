# AI Email Response Drafting
*AI Email Response Drafting* is a modular system designed to streamline email communications for State and Local government organizations. It monitors designated inboxes, intelligently drafts responses using AI, and ensures every reply is grounded in agency-specific knowledge and guidance.

Originally developed in collaboration with a major state-level electric grid operator, this system is built to support high-volume customer inquiry scenarios where accuracy, transparency, and responsiveness are critical.

This system empowers public sector teams to:
- **Automate initial email response drafting** using AI trained on organization-provided materials
- Maintain accuracy and compliance with a built-in **human-in-the-loop review** interface
- **Save time** while preserving oversight, tone, and trust in every outgoing message

Whether you're managing constituent inquiries, service requests, or internal communications, this tool helps your team respond faster, without sacrificing quality or control.

**Click the image below for a short demo!**

[![play](https://i.imgur.com/E6skQrw.png)](https://youtu.be/-FJS5UpcpiM)

## How it works: Behind the Scenes
This combined system uses a hybrid and diverse architecture of Microsoft Power Platform and Azure AI Resources:

![architecture](https://i.imgur.com/O7MmEbv.png)

- An **Outlook inbox** is monitored for incoming emails
- A **Power Automate workflow** triggers upon the arrival of new inquiries via email
- A language model hosted in **Azure AI Foundry** is prompted for the drafting of a response email
- An **Azure AI Search** instance is used to find pertinent organizational knowledge in **Azure Blob Storage** that is relevant to the question, and thus will be used to draft the response
- A staff member uses a **Power App** to review received emails, drafted response, can make modifications, and send directly from a consolidated interface

### Power Automate Workflow
A Power Automate workflow serves as an orchestration layer for the entire process. The workflow is triggered upon an email arriving, prompts a language model in Azure AI Foundry, and saved both the original email inquiry and proposed response to Dataverse (for later review).

![Power Automate](https://i.imgur.com/HzKqX13.jpeg)

### Azure AI Foundry: Language Model
A language model, GPT-4.1 used in this example, is deployed in Azure AI Foundry and serves as the "brain", authoring the proposed response.

![LLM](https://i.imgur.com/Pjn2ADk.jpeg)

### Knowledge In Blob Storage
The organization can upload all knowledge material that should be referenced for answers to all questions to a Azure Blob Storage container. Several documents uploaded below as an example.

![blob](https://i.imgur.com/vhwiz2U.jpeg)

### Azure AI Search
An Azure AI Search instance indexes all content within the Azure Blob Storage container, enabling search as a RAG pattern.

![indexer](https://i.imgur.com/1KI8BTB.jpeg)

![indexer tested](https://i.imgur.com/zOtrv1s.jpeg)

### Azure AI Foundry Language Model Tested
As seen below in a simple HTTP call test to the language model in Azure AI Foundry, we provide the model with:
- Instructions
- A question (inquiry)
- A data source to leverage for knowledge

And it responds with:
- A response draft
- References as to what knowledge it used in the response

![test](https://i.imgur.com/HF2V2AO.jpeg)

### Display to Staff in Power Apps
With the inqury email and proposed response stored in Dataverse by the Power Automate workflow, a human can then review in a Power App interface.

![power app](https://i.imgur.com/Z2QMHNs.jpeg)

Once they are ready to send, the Office 365 Outlook connector in Power Apps allows for seamless sending without leaving this interface.

![sending in power apps](https://i.imgur.com/rU9YqaT.jpeg)

## Solution File
You can download the Power Platform solution file [here](https://github.com/microsoft/SLG-Business-Applications/releases/download/47/AIEmailDrafting_1_0_0_2.zip).

To install:
1. Import this solution into a Power Platform environment
2. Azure Deployments:
    1. Deploy an Azure Storage account instance and upload pertinent knowledge to a container
    2. Deploy an Azure AI Search instance, configure, and schedule an indexer on the content in BLOB storage.
    3. Deploy an Azure OpenAI LLM (i.e. GPT-4.1, GPT-5, etc.) in Azure AI Foundry.
3. In the *"Orchestrate AI Email Response"* Power Automate flow, replace [these fields](https://i.imgur.com/rT7Phlo.png) with:
    - Direct URL to your Azure AI Foundry LLM deployment
    - Replace "SEGO" with your organization's name
    - Endpoint to your Azure AI Search instance
    - API key to your Azure AI Search instance

## Misc Assets
- [SEGO Logo](https://i.imgur.com/9iL0z3c.png) (ficticious organization)

## Credit
Made by [Tim Hanewich](https://github.com/TimHanewich).