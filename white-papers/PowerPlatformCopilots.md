# Power Platform Copilot Feature Availability in GCC
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

## Power Platform Copilots
The sections and tables that follow identify all known Power Platform Copilot features.  The tables show to availability of each feature in both commercial cloud and in GCC.  The Dates shown are when each feature's availability was last evaluated.

> [!IMPORTANT] 
> There's now a central Microsoft documentation page that outlines all Dynamics 365 and Power Platform Generative AI features and the controls you have for each of them.  That document can be found in the link below,
> 
> [Copilot in Dynamics 365 apps and Power Platform Governance Controls](https://learn.microsoft.com/en-us/power-platform/faqs-copilot-data-security-privacy#copilot-in-dynamics-365-apps-and-power-platform)

* GA = Generally Available
* "Early Access" is a phase that may come prior to "Preview".

### Power Apps - Canvas Apps
Maker Copilot features
|Feature|Commercial|GCC|Date|
|-|-|-|-|
|[Explain this PowerFX formula](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-formulas-formulabar#explain-a-formula)|GA|Yes|10.18.24|
|[Generate formula from code comments](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-formulas-formulabar#generate-formulas-from-code-comments-preview)|Preview|No|10.18.24|
|[Build apps through conversation](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-conversations-create-app)|GA|No|10.18.24|
|[Field suggestions by Copilot](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/ai-field-suggestions)|GA|No|10.18.24|
|[Natural language to formulas](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/power-apps-ideas-transform)|GA|No|10.18.24|
|[Create an app description with Copilot](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/save-publish-app#create-an-app-description-with-copilot-preview)|Preview|No|10.21.24|

User Copilot features
|Feature|Commercial|GCC|Date|
|-|-|-|-|
|[Copilot control (open-ended chat)](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/add-ai-copilot)|Preview|No|10.18.24|
|[Copilot answer control (Pre-baked prompt)](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/copilot-answer-control-overview)|Preview|No|10.18.24|
|[Add a custom copilot to a canvas app](https://learn.microsoft.com/en-us/power-apps/maker/canvas-apps/add-custom-copilot)|Preview|No|10.18.24|
|[Filter, sort, search galleries with Copilot](https://learn.microsoft.com/en-us/power-apps/user/smartgrid)|Preview|No|10.3.24|
|[Draft well-written input text with Copilot](https://learn.microsoft.com/en-us/power-apps/user/well-written-input-text-copilot)|Preview|No|10.3.24|

### Power Apps - Model Driven Apps
Maker Copilot features
|Feature|Commercial|GCC|Date|
|-|-|-|-|
|[Column suggestions by Copilot](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/create-and-edit-forms#column-suggestions-by-copilot-preview)|Preview|No|10.18.24|
|[Create an app description with Copilot](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/build-first-model-driven-app#create-an-app-description-with-copilot-preview)|Preview|No|10.21.24|

User Copilot features
|Feature|Commercial|GCC|Date|
|-|-|-|-|
|[Copilot in rich text editor](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/copilot-control)|Early Access|No|10.18.24|
|[Copilot in email rich text editor](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/use-copilot-email-assist)|Early Access|No|10.18.24|
|[Copilot for App Users](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/add-ai-copilot)|Preview|No|10.18.24|
|[Form filling assistance with Copilot](https://learn.microsoft.com/en-us/power-apps/user/form-filling-assistance)|Preview|No|10.18.24|
|[Timeline highlights](https://learn.microsoft.com/en-us/power-apps/user/add-activities#use-timeline-highlights-powered-by-generative-ai)|GA|No|10.18.24|

### Power Automate
|Feature|Commercial|GCC|Date|
|-|-|-|-|
|[Describe to design](https://learn.microsoft.com/en-us/power-automate/create-cloud-flow-from-description)|Preview|Yes|9.15.24|
|[Edit a flow with Copilot](https://learn.microsoft.com/en-us/power-automate/get-started-with-copilot#edit-a-flow-using-the-designer-with-copilot-capabilities)|Preview|Yes|10.18.24|
|[Troubleshoot in Copilot](https://learn.microsoft.com/en-us/power-automate/fix-flow-failures#troubleshoot-in-copilot)|GA|No|10.18.24|

### Power Pages
|Feature|Commercial|GCC|Date|
|-|-|-|-|
|[Site creation with Copilot](https://learn.microsoft.com/en-us/power-pages/getting-started/create-site-copilot)|GA|Yes|10.21.24|
|[Add Copilot to Power Pages site](https://learn.microsoft.com/en-us/power-pages/getting-started/enable-chatbot)|GA|Yes|10.21.24|
|[Add AI-generated text](https://learn.microsoft.com/en-us/power-pages/getting-started/add-text-copilot)|GA|Yes|10.21.24|
|[Add an AI-generated form](https://learn.microsoft.com/en-us/power-pages/getting-started/add-form-copilot)|GA|Yes|10.21.24|
|[Add an AI-generated multi-step form](https://learn.microsoft.com/en-us/power-pages/getting-started/multistep-forms-copilot)|Preview|Yes|10.21.24|
|[Create AI-generated theme](https://learn.microsoft.com/en-us/power-pages/getting-started/theme-copilot)|GA|Yes|10.21.24|
|[Add AI-generated code](https://learn.microsoft.com/en-us/power-pages/configure/add-code-copilot)|Preview|||
|[Power Pages Search with gen AI](https://learn.microsoft.com/en-us/power-pages/configure/search/generative-ai)|Preview|No|10.21.24|
