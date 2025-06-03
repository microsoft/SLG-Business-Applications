# Dynamics 365 and Power Platform Copilot Feature Availability in GCC
> [!IMPORTANT]
> November 7, 2024 through Nov 12, 2024 we are going live with Phase 2 of Copilot availbility for Power Platform and Dynamics 365 in GCC and GCC High!  For more information on what this means, [go here](https://github.com/microsoft/Federal-Business-Applications/blob/main/whitepapers/copilot/README.md).

The goal of this whitepaper is to serve as a guide for all US state and local government customers to understand the availability of various Power Platform Copilot features in GCC.  We also want to outline our Microsoft Responsible AI framework.  

This document will continue to evolve as we roll out more features and functionality over time.

## Responsible AI
Every Microsoft Business Applications service has their own dedicated Responsible AI page on our public documentation site.  Below we have summarized the links for all Power Platform and Dynamics 365 services and their associated Responsible AI page.

* [FAQ for Copilot data security and privacy for Dynamics 365 and Power Platform](https://learn.microsoft.com/en-us/power-platform/faqs-copilot-data-security-privacy)
* [Power Platform](https://learn.microsoft.com/en-us/power-platform/responsible-ai-overview)
  * [AI Builder](https://learn.microsoft.com/en-us/ai-builder/responsible-ai-overview)
  * [Copilot Studio](https://learn.microsoft.com/en-us/microsoft-copilot-studio/responsible-ai-overview)
  * [Power Apps](https://learn.microsoft.com/en-us/power-apps/maker/common/responsible-ai-overview/)
  * [Power Automate](https://learn.microsoft.com/en-us/power-automate/responsible-ai-overview/)
  * [Power Pages](https://learn.microsoft.com/en-us/power-pages/responsible-ai-overview/)
* [Dynamics 365](https://learn.microsoft.com/en-us/dynamics365/responsible-ai-overview)
  * [Dynamics 365 Customer Service](https://learn.microsoft.com/en-us/dynamics365/customer-service/implement/responsible-ai-overview)
 
## Azure OpenAI
Azure OpenAI is the service that all Copilot features are built upon. 

> [!IMPORTANT] 
> Your prompts (inputs) and completions (outputs), your embeddings, and your training data:
> 
> * are NOT available to other customers.
> * are NOT available to OpenAI.
> * are NOT used to improve OpenAI models.
> * are NOT used to improve any Microsoft or 3rd party products or services.
> * are NOT used for automatically improving Azure OpenAI models for your use in your resource (The models are stateless, unless you explicitly fine-tune models with your training data).
> 
> Your fine-tuned Azure OpenAI models are available exclusively for your use.
The Azure OpenAI Service is fully controlled by Microsoft; Microsoft hosts the OpenAI models in Microsoft’s Azure environment and the Service does NOT interact with any services operated by OpenAI (e.g. ChatGPT, or the OpenAI API).

For a full description of Azure OpenAI's data, privacy and security details, you can find them in the link below,

* [Data, privacy, and security for Azure OpenAI Service](https://learn.microsoft.com/en-us/legal/cognitive-services/openai/data-privacy)

## Dynamics 365 and Power Platform Copilots
The sections and tables that follow identify all known Copilot features in Dynamics 365 Customer Service and Power Platform.  The tables show to availability of each feature in both commercial cloud and in GCC.
> [!IMPORTANT] 
> There's now a central Microsoft documentation page that outlines all Dynamics 365 and Power Platform Generative AI features and the controls you have for each of them.  That document can be found in the link below,
> 
> [Copilot in Dynamics 365 apps and Power Platform Governance Controls](https://learn.microsoft.com/en-us/power-platform/faqs-copilot-data-security-privacy#copilot-in-dynamics-365-apps-and-power-platform)

* The Dates shown are when each feature's availability was last evaluated.
* GA = Generally Available
* "Early Access" is a phase that may come prior to "Preview".

## Dynamics 365 Copilots

### Dynamics 365 Customer Service Copilots
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Case summary](https://learn.microsoft.com/en-us/dynamics365/customer-service/administer/copilot-enable-summary#enable-case-summaries)|GA|Yes|10.23.24|
|[Live conversation summary](https://learn.microsoft.com/en-us/dynamics365/customer-service/administer/copilot-enable-summary#enable-conversation-summaries)|GA|||
|[Translation](https://learn.microsoft.com/en-us/dynamics365/customer-service/administer/copilot-enable-help-pane#enable-translation)|GA|Yes|10.23.24|
|[Create a case from conversation](https://learn.microsoft.com/en-us/dynamics365/customer-service/use/copilot-use-summary#get-a-conversation-summary:~:text=Copy%20the%20summary.-,Select%20Create%20case,-to%20create%20a)|GA|||
|[Draft a response](https://learn.microsoft.com/en-us/dynamics365/customer-service/use/use-copilot-features#draft-a-chat-response-preview)|Preview|No|10.23.24|
|[Write an email](https://learn.microsoft.com/en-us/dynamics365/customer-service/use/use-copilot-email?tabs=richtexteditor)|GA|Yes|10.23.24|
|[Ask a question (knowledge search)](https://learn.microsoft.com/en-us/dynamics365/customer-service/use/use-copilot-features#ask-a-question)|GA|Yes|10.23.24|
|[Plugins for generative AI](https://learn.microsoft.com/en-us/dynamics365/customer-service/administer/enable-copilot-plugins-for-generative-ai)|Preview|Preview|10.23.24|
|[Suggest knowledge from cases](https://learn.microsoft.com/en-us/dynamics365/customer-service/use/use-copilot-knowledge-from-cases)|Preview|No|10.23.24|


## Power Platform Copilots

### Power Apps - Canvas Apps
Maker Copilot features
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Explain this PowerFX formula](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-formulas-formulabar#explain-a-formula)|GA|Yes|10.18.24|
|[Generate formula from code comments](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-formulas-formulabar#generate-formulas-from-code-comments)|GA|No|3.17.25|
|[Create a formula](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-formulas-formulabar#create-a-formula-preview)|Preview|No|3.17.25|
|[Build apps through conversation](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-conversations-create-app)|GA|No|3.17.25|
|[Edit your app with Copilot](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-edit-app)|Preview|No|3.17.25|
|[Field suggestions by Copilot](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-field-suggestions)|GA|No|3.17.25|
|["Ideas" - Natural language to formulas for galleries](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/power-apps-ideas-transform)|GA|Yes|3.17.25|
|[Create an app description with Copilot](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/save-publish-app#create-an-app-description-with-copilot-preview)|Preview|No|3.18.25|

User Copilot features
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Copilot control (open-ended chat)](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/add-ai-copilot)|Preview|No|3.18.25|
|[Copilot answer control (Pre-baked prompt)](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/copilot-answer-control-overview)|Preview|No|3.18.25|
|[Add a custom copilot to a canvas app](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/add-custom-copilot)|Preview|No|3.18.25|
|[Filter, sort, search galleries with Copilot](https://learn.microsoft.com/en-us/power-apps/user/smartgrid)|Preview|No|3.18.25|
|[Draft well-written input text with Copilot](https://learn.microsoft.com/en-us/power-apps/user/well-written-input-text-copilot)|Preview|No|3.18.25|

### Power Apps - Model Driven Apps
Maker Copilot features
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Create, edit and configure forms using Form Designer](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/create-and-edit-forms)|GA|No|6.3.25|
|[Create an app description with Copilot](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/build-first-model-driven-app#create-an-app-description-with-copilot)|GA|No|6.3.25|

User Copilot features
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Copilot in email rich text editor](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/use-copilot-email-assist)|GA|No|6.3.25|
|[Copilot for App Users](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/add-ai-copilot)|GA|No|6.3.25|
|[Form filling assistance with Copilot](https://learn.microsoft.com/en-us/power-apps/user/form-filling-assistance)|Preview|No|6.3.25|
|[Timeline highlights](https://learn.microsoft.com/en-us/power-apps/user/add-activities#use-timeline-highlights-powered-by-generative-ai)|GA|No|3.18.25|

### Power Automate
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Describe to design](https://learn.microsoft.com/en-us/power-automate/create-cloud-flow-from-description)|GA|Yes|9.15.24|
|[Edit a flow with Copilot](https://learn.microsoft.com/en-us/power-automate/get-started-with-copilot#edit-a-flow-using-the-designer-with-copilot-capabilities)|GA|Yes|10.18.24|
|[Troubleshoot in Copilot](https://learn.microsoft.com/en-us/power-automate/fix-flow-failures#troubleshoot-in-copilot)|GA|No|10.18.24|

### Power Pages
|Feature|Commercial|GCC|Eval Date|
|-|-|-|-|
|[Site creation with Copilot](https://learn.microsoft.com/en-us/power-pages/getting-started/create-site-copilot)|GA|Yes|10.21.24|
|[Add Copilot to Power Pages site](https://learn.microsoft.com/en-us/power-pages/getting-started/enable-chatbot)|GA|Yes|10.21.24|
|[Add AI-generated text](https://learn.microsoft.com/en-us/power-pages/getting-started/add-text-copilot)|GA|Yes|10.21.24|
|[Add an AI-generated form](https://learn.microsoft.com/en-us/power-pages/getting-started/add-form-copilot)|GA|Yes|10.21.24|
|[Add an AI-generated multi-step form](https://learn.microsoft.com/en-us/power-pages/getting-started/multistep-forms-copilot)|Preview|Yes|10.21.24|
|[Create AI-generated theme](https://learn.microsoft.com/en-us/power-pages/getting-started/theme-copilot)|GA|Yes|10.21.24|
|[Add AI-generated code](https://learn.microsoft.com/en-us/power-pages/configure/add-code-copilot)|Preview|No|10.23.24|
|[Power Pages Search with gen AI](https://learn.microsoft.com/en-us/power-pages/configure/search/generative-ai)|Preview|No|10.21.24|

### Copilot Studio
|Feature|Commercial|GCC|Eval Date|Note|
|-|-|-|-|-|
|[Generative answers](https://learn.microsoft.com/en-us/microsoft-copilot-studio/nlu-gpt-overview#generative-answers)|GA|Yes|10.24.24||
|[AI general knowledge](https://learn.microsoft.com/en-us/microsoft-copilot-studio/nlu-gpt-overview#ai-general-knowledge)|GA|Yes|10.24.24||
|[Orchestrate copilot topics and actions with generative AI](https://learn.microsoft.com/en-us/microsoft-copilot-studio/advanced-generative-actions)|Preview|Yes|10.24.24||
|[Create a Copilot with natural language](https://learn.microsoft.com/en-us/microsoft-copilot-studio/fundamentals-get-started?tabs=web#create-a-copilot)|GA|No|10.24.24||
|[Create and edit topics with natural language](https://learn.microsoft.com/en-us/microsoft-copilot-studio/fundamentals-get-started?tabs=web#create-a-copilot)|GA|Yes|10.24.24||
|[Azure Open AI as a knowledge source for generative answers](https://learn.microsoft.com/en-us/microsoft-copilot-studio/nlu-generative-answers-azure-openai)|Preview|No|3.18.25|[Custom pattern available](https://github.com/microsoft/SLG-Business-Applications/tree/main/white-papers/Copilot-Studio%2BAzure-AI-GCC)|
|[Autonomous Agents - Triggers](https://learn.microsoft.com/en-us/microsoft-copilot-studio/authoring-triggers-about)|Preview|No|3.18.25||
|[Agent Builder for M365 Copilot](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/copilot-studio-agent-builder-build)|GA|No|3.18.25||

### Platform
|Feature|Commercial|GCC|Eval Date|Notes|
|-|-|-|-|-|
|[Dataverse Data Workspace](https://www.microsoft.com/en-us/power-platform/blog/power-apps/data-workspace-is-now-generally-available/?msockid=30d3025b988b65e001ec13a7997364df)|GA|No|6.3.25||
|[Dataverse AI functions](https://learn.microsoft.com/en-us/power-platform/power-fx/reference/function-ai)|GA|No|6.3.25|Use AI Builder instead|
|[AI Builder - AI prompts](https://learn.microsoft.com/en-us/ai-builder/prompts-overview)|GA|Yes|10.24.24||
|[Power Designer - Copilot-first development](https://learn.microsoft.com/en-us/power-apps/maker/plan-designer/plan-designer)|GA|No|6.3.25|
