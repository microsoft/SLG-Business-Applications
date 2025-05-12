# Document Automation Toolkit
The Document Automation Toolkit allows you to easily create a comprehensive document processing solution using AI Builder, Power Automate, Power Apps, and Microsoft Dataverse.

Power Automate orchestrates the overall process while AI Builder brings the intelligence required to efficiently extract information from structured documents. Power Apps allows users to manually review and approve documents, and Dataverse handles the back end.

For additional learning materials, go to [Training: Automate the processing of documents with the AI Builder prepackaged solution (module)](https://learn.microsoft.com/en-us/training/modules/get-started-ai-builder-document-automation/).

## Premise and value
Forms are ubiquitous in government for both back office and line of business processes.  While governments work diligently to digitize legacy forms, countless forms remain paper-based or "semi-digitized".  The Document Automation Toolkit presents an opportunity to quickly and easily automate the processing of such forms, thereby accelerating the process, reducing errors, and freeing up staff hours for more valuable tasks.

The basic premise is this.  Forms are used to collect information.  But that information isn't much good until it is extracted from the original form and fed into the corresponding business system/process.  This extraction and input process is often a mundane task performed by an overqualified human whose time would be better spent elsewhere.  So let's automate that bit and free our humans!

## How to deploy and configure the base kit
Good news - it's incredibly easy!

If you operate in commercial cloud you can access the [document automation toolkit](https://flow.microsoft.com/manage/aibuilder/documentautomation) in Power Automate.

If you operate in the Government Community Cloud (GCC), download and install the [managed solution file](https://github.com/microsoft/SLG-Business-Applications/releases/download/20/Documentautomationbasekit_managed.zip).

You'll also need to train a Document Processing model in AI Builder to understand your specific form(s).  Unless you happen to be processing vendor invoices or travel expense receipts in which case, you're in luck as there are pretrained models available for these purposes!  Be sure to publish your model once you've trained it.

Finally, you'll run the Document Automation Application (part of the base kit) and link it to your Document Processing model.  The Toolkit will automatically recognize - and find a home in Dataverse for - the data fields your model has been trained to extract.

This is all pretty easy and we have quick video to usher you through the process:

[![demo video](https://i.imgur.com/0VMEFlb.png)](https://livesend.microsoft.com/i/xcwGbDD43wDLvKjfycYRxERQLNptUXYoYxqmsqhj6a9Diz4aqhJ0qqS1zl3M4S6kjRYmi___GxcJovuHfPKSREEcF0Kzhrt20uZnkq76HpYjj7AF9Dwg7xeqahqNqDDpzJ)

## How to put the base kit in action
The Document Automation Toolkit is a collection of Power Platform artifacts, which you'll see in the Solution you imported.  The preconfigured automation will trigger whenever an email with attachments is sent to your (the solution installer's) email inbox.  To test the automation, simply send an email to yourself with one or more samples of completed forms attached.  The automation will do the rest and you'll quickly see the attachments as records in the Document Processing Application.

## Best practices for customizing/extending the base kit
You may want to layer some additional conditions on the initial trigger and/or change the trigger action entirely.  Perhaps you only want to trigger the automation when the email subject contains a certain string of text.  Or perhaps you'd rather trigger the automation from a shared mailbox or when a file is added to a SharePoint library.

You may also wish to create additional automation that'll take verified data and move it to a line of business application.

The base kit you installed is a Managed Solution and does not support customization. For any type of customization/extension, you'll want to create a new Power Platform Solution to work within. You can import Power Automate Flows from the base kit into this Solution for customization, and create net new Power Automate Flows directly within this Solution.

## FAQ

### What kind of forms are good candidates for this process?
This toolkit is intended for "fixed template" documents (i.e. forms that adhere to a consistent layout). The underlying document processing AI model can handle complexities like imbedded tables and handwritten responses and is detailed on [Microsoft Learn](https://learn.microsoft.com/en-us/ai-builder/form-processing-model-overview).  Be sure to reference the [requirements/limitations](https://learn.microsoft.com/en-us/ai-builder/form-processing-model-requirements#requirements) and the [FAQ](https://learn.microsoft.com/en-us/ai-builder/form-processing-faq).

### Can I use this toolkit for vendor invoice automation?
Yes! Vendor invoice formats are varied and complex, but there's an out of the box Document Processing model for vendor invoice processing - which can be used within Document Automation - so the base kit configuration will actually be expedited.

### What do I do with the form data once it's extracted?
Most likely you'll want to create a custom Power Automate that triggers when a document's status is set to "Validated".  The Flow will grab the extracted data elements out of Dataverse and then use additional actions/connectors to move this data to the appropriate target.

### What if I want manual review even when the AI confidence is high?
The Document Automation Validator flow checks the confidence scores returned by AI Builder for each data element. If the confidence score for all fields is greater than 80%, the status if the document itself is set to "Validated".  Documents with lower confidence are set to "Manual Review" and await manual validation via the Document Processing Application.

To change this behavior, add the Document Automation Validator flow to a new Solution and change the "happy path" outcome to "Manual Review".

### What if I want to automate several different forms?
Using the concept of [Document Collections](https://learn.microsoft.com/en-us/ai-builder/create-form-processing-model#group-documents-by-collections), it's possible to train a single model for multiple document formats.  This makes sense in the case of something like vendor invoices where we'll be ingesting multiple formats but extracting a common set of data elements.  

In cases of independent, unrelated forms that do not contain overlapping data elements, it's best to create separate AI Builder models. With multiple AI Builder models in a single environment, additional configuration of the Power Automate Flows will be required.

### Do I need Premium licensing to use the Document Automation Toolkit?
Yes.  The solution uses Dataverse and Premium Connectors, therefore all users must be licensed for Power Apps Premium.

### How is AI Builder licensed?
AI Builder is licensed on a credit consumption model.  Power Apps/Automate Premium licenses include a modest amount of AI Builder credits.  Chances are, it's at least enough to get you started. When in doubt on licensing, get in touch with your Microsoft contacts.
