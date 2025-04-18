# Connect Copilot Studio to Azure AI in GCC
Copilot Studio is a fantastic no-code / low-code tool for creating custom chatbot agents armed with Generative AI reasoning over your data.  Within minutes, a no-code maker can build a Generative AI agent that'll extract knowledge from specific documents, public websites, SharePoint repositories and other data sources.

At times, use cases demand more control of the LLM, AI Search indexes, knowledge sources, etc., and customers elect to provision and manage their own Azure AI resources.  This whitepaper provides guidance on connecting your Copilot Studio agent to customer-managed Azure AI resources.

## Why do I have to configure this myself?
Good question.  The short answer is, "Government Community Cloud (GCC)".  In commercial cloud you can use the Azure/Copilot Studio GUIs to make this type of connection.  It will come as no surprise to seasoned practitioners that things aren't always quite as easy in GCC.  GCC nuance aside, this approach may prove attractive simply because it offers full control of the API request, the manipulation of the JSON response, and the presentation to the end user.  We're pairing the convenience and versatility of Copilot Studio with full control of the Generative AI actions and resources.

## Solution architecture
There are multiple variations possible, but the core to this approach is Copilot Studio calling a Power Automate Flow, which then calls an Azure AI API and responds back to Copilot Studio.  The end-to-end architecture looks something like this, where the user interacts with a Copilot Studio agent deployed in Teams, on a website, or through another distribution channel, some or all users requests trigger the Power Automate which then replies back to the user via Copilot Studio.

![Copilot Studio plus Azure AI simple architecture](https://i.imgur.com/RpmqXw4.png)

## How to: Power Automate Configuration
First, configure your Azure AI resources using [Using your data with Azure Open AI Service](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/use-your-data?tabs=ai-search%2Ccopilot) or equivalent approach.  The critical resources are an LLM deployment that supports the "your data" API, a suitable AI Search resource, and [likely] a storage account.  This covers the right side of the above diagram.

Next, we address the Power Platform components.  Download the [Unmanaged Solution file](https://github.com/microsoft/SLG-Business-Applications/releases/download/19/TemplateAzureAIforCopilotStudio_1_0_0_3.zip) and import it into your target environment.  The Solution contains two Power Automate Flows and a Dataverse table.

Power Automate is the workhorse in this model as it facilitates the communication between Copilot Studio and Azure AI. In a moment, we'll configure Copilot Studio to pass users' questions to one of these two Power Automate Flows, and to return Power Automate's responses back to the Copilot user.  Power Automate will take the user's question, pass it along to customer-owned Azure AI resources, and parse the JSON response to construct a reply for Copilot Studio.

There is a subtle but important difference between the two Power Automate Flows.  The "[Template] Azure AI for Copilot Studio" flow sends each request as an isolated prompt that knows nothing of the previous conversation context.  The "[Template] Azure AI for Copilot Studio - with history" flow incorporates a model where conversation history is stored in a custom Dataverse table (specifically, the "AOAI message" table, which is part of the Solution file) and is recalled before sending the Azure AI request.  This approach makes Azure AI aware of the conversation's context and may yield better results.

> [!Important]
> The solution file now contains THREE Power Automate Flows - see the section, "Working with reference citations" for an explanation.

Note: Copilot Studio automatically stores conversation transcripts.  However, these records are not updated until the conversation ends and an alternate approach is required for near real time conversation tracking.  Users of the "multiturn" model may choose to periodically drop the records in the "AOAI message history" table periodically to mitigate Dataverse storage consumption.

Both Power Automate Flows use an HTTP action to send the request to Azure Open AI.  You will need to edit the target Flow(s) and update the parameters for the HTTP action.  You'll need the URI and API key for your LLM, plus the AI Search resource, the search index name and API key for the search resource.

![Azure AI HTTP request](https://i.imgur.com/Sbl8b7a.png)

The rest of the Flow will parse the API response and format a text string to return to Copilot Studio.  This text string will include the name of, and a link to, the source documents used by Azure AI to generate the main content of the response.  (Note: this aspect is subject to the availability of those data elements in the API response.)

## Working with the reference citations
As of April, 2025, the template solution has been updated with an improved pattern for citing references.  Copilot Studio observes markdown syntax when presenting messages to users.  We can manipulate the appearance of our Azure AI generated responses by adhering to the expected markdown format for citing references.  The expected format for a URL-based reference is as follows:

```
This is the Azure AI generated response [doc4].

[doc4]: http://www.your_target_url "the display name for this reference"
```

If Power Automate's reply to Copilot Studio adheres to this format, the user will receive a nicely formatted response.  The solution template now contains an updated Flow called "[Template] Azure AI for Copilot Studio - with history and improved references", which shows a pattern for doing just that.

In examining this pattern one might notice AI Builder being used.  What's that about?  Azure AI often references more sources than it ultimately used to generate its response.  AI Builder is a novel way of identifying which sources were actually used by Azure AI and providing Power Automate a means by which to extract those references from the Azure AI API response.  Take a look at the prompt in the solution file - it's pretty cool!


## How to: Copilot Studio Configuration
Next, we'll configure your Copilot Studio agent.  In simple scenarios, we'll be fully surprising the native generative AI capabilities within Copilot Studio and replacing them with an action to call our Power Automate Flow.

In your agent, go to Topics and find the Conversational Boosting topic (under system topics).  This topic is triggered on unknown intent and, by default, uses the Generative Answers action to respond to questions using Generative AI.  We're going to adjust the structure of this topic by removing the Generative Answers node, and the condition that follows, and replacing them with our Power Automate Flow.  To insert the flow, add a node to the topic designer, use the "Call an action" menu item, and select the appropriate Power Automate Flow from the list of Basic actions.  Populate the input parameter(s) using System variables and return the Response output to the user in a subsequent message.  When complete, your new Conversational Boosting topic will look like this:

![Conversational boosting topic](https://i.imgur.com/pvoZ2Ga.png)

Save the topic updates, refresh your test pane, and ask away! Successful results will resemble the following. If an error is returned, there's most likely an issue with the HTTP action in Power Automated.  Return to your Solution and examine the run history to troubleshoot.

## Message appearance using updated reference behavior
![Test pane in new format](https://i.imgur.com/Kk4wb2J.png)

## Message appearance using original reference behavior
![Test pane in old format](https://i.imgur.com/Pdt1UcC.png)
