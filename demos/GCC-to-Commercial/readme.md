# Power Platform GCC + Azure Commercial Integration

## This Specific Example is Obsolete
This example demonstrates how to facilitate cross-cloud communication, calling upon an Azure BLOB Storage service running in Azure Commercial, from a Power Platform environment running in Azure Government ("GCC"). 

Since the [Azure BLOB Storage connector](https://learn.microsoft.com/en-us/connectors/azureblob/) now natively supports connecting to services living in another Azure Cloud, you can simply use this capabilitiy instead of the workaround showcased below.

However, for other connectors that do *not* yet support cross-cloud, this **architecture is still suggested as a workaround**. You can read further on exactly which connectors do and do not support cross-cloud in our white paper [here](../../white-papers/azure-connectors/).

## The Issue
When working with Power Platform/Dynamics 365 resources in a GCC environment, integrations with Azure resources (nearly) always point to the Azure Government endpoints of resources. Many customers have an Azure Commercial instance paired with their Business Applications GCC environment, thus they are prevented from connecting with their Azure resources using the Power Platform's native SaaS integrations.

A more thorough explanation of this issue can be found [here](https://github.com/microsoft/Federal-Business-Applications/blob/main/whitepapers/power-plat-d365-architecture/README.md#gcc-architecture).

## The Solution (workaround)
*Click the image below to watch a short video of the finished solution*  
[![demonstration video](https://i.imgur.com/y4h6MRJ.jpg)](https://youtu.be/BIdi-6Fiics)  
This demonstrates how a Power Apps/Power Automate resource residing in a GCC environment can indirectly communicate with an Azure Commercial resource living in the same tenant using an API (an Azure Function used here) and custom connectors.

The following Power Platform GCC --> Azure Commercial actions are demonstrated here:
- From within a GCC Power App, uploading a file to Azure Commercial Blob Storage
- From within a GCC Power App, downloading a file from Azure Commercial Blob Storage
- From within a GCC Power Automate flow, subscribing to a "when a blob is created" event in Azure Commercial Blob Storage (webhook)

You can find an architecture diagram and description for each demonstrated scenario below:

### File Upload to Commercial Blob Storage
![upload](https://i.imgur.com/dK3rHkE.png)

### File Download from Commercial Blob Storage
![download](https://i.imgur.com/uTav3wu.png)

### Subscribing to "when a blob is created" event
![subscribe](https://i.imgur.com/xAyxlo8.png)
This process is more complicated than the first two examples. While the first two examples (data upload and data download) are simplistic and only require a single call and response, this requires several stages. To describe them at a high level:
1. The `eventbroker` endpoint (function) subscribes to Azure Blob Storage's events, specifically, the "blob created" event. This is not an automatic process. You will trigger the subscription manually in Azure Blob Storage. Blob storage will send a verification URL to the endpoint URL you are trying to subscribe with. It will send a verification URL in a POST message. Your endpoint must make a GET call to this URL to confirm registration and complete the "handshake". The code in the [eventbroker function](./api/webhooks/EventBroker.cs) is designed to detect this verification, find the URL, and make the GET request, but for some reason I couldn't get it to work. The function will log status as it progresses through. You can grab the URL from the messages and manually make an HTTP GET request to it in your browser to complete the handshake.
2. Your Power Automate workflow (residing in GCC) will subscribe to the "When an Azure blob is created" trigger. Behind the scenes of the custom connector, the `subscribe` endpoint is being hit with the unique URL that is to be POST'ed to when a blob is created. The `subscribe` function stores this unique URL in a JSON-serialized list of subscribers that is stored in blob storage.
3. When a blob is made in blob storage, Azure Blob Storage (commercial) will call to the `eventbroker` endpoint, notifying it of this new blob. 
4. The `eventbroker` endpoint will retrieve the JSON-serialized list of subscribers and make a simplified POST request to each of them with information about the new blob that was made (thus triggering the Power Automate flow).
5. When the Power Automate workflow that has this trigger (has been subscribed) is turned off or deleted, an HTTP DELETE request is sent to the `unsubscribe` endpoint, asking it to be removed from the list of subscribers. The [unsubscribe function](./api/webhooks/unsubscribe.cs) provided here works with some simple URL's but does not work with more complicated URL's (like the ones used in Power Automate triggers). This is probably because of the URL deserialization not working. Ideally, there should be an ID number of some sort that corresponds to an individual subscription and can be used to unsubscribe, but that would be too complicated for this simple demonstration. For now, understand that the `unsubscribe` endpoint doesn't always work and deleted workflow triggers may still be listed there.

## Credit
Created by [Tim Hanewich](https://github.com/TimHanewich).