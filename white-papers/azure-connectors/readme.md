# Using Azure Connectors in Power Platform GCC

The goal of this whitepaper is to provide guidance on working with Azure connectors in the US Government sovereign cloud. This document will evolve as additional updates become available.

### Background

An understanding of underlying architectural components is helpful for appreciating why this whitepaper is necessary. Most GCC customers view Azure Commercial as their natural landing zone for Azure Subscriptions and Resources. This is largely due to their Entra ID (formerly Azure Active Directory) presence residing in Azure Commercial and the beyond-adequate security and compliance assurances offered by Azure Commercial.

However, GCC customers using Power Platform and/or Dynamics 365 are also consumers of Azure Government, as the GCC versions of these services run in Azure Government. This reality is largely abstracted from the customer's point of view as these services run in "Microsoft's Azure Subscription" and are served-up Software as a Service (SaaS) style to end customers.

![GCC Architecture](https://github.com/microsoft/Federal-Business-Applications/blob/main/whitepapers/power-plat-d365-architecture/files/Slide2.PNG)
Image credit: [Steve Winward](https://github.com/microsoft/Federal-Business-Applications/tree/main/whitepapers/power-plat-d365-architecture)

### Impact

In certain instances this architecture leads to cross-cloud communication considerations. Consider Power Automate, for example. Power Automate is largely an abstraction of Azure Logic Apps where Power Automate GCC builds upon Logic Apps in Azure Government. As such, when Power Automate GCC goes to make a connection to customer-owned Azure resources, it's only natural for it to seek out other resources in Azure Government. This of course leads to consternation for many GCC customers who are primarily (or exclusively) consumers of Azure Commercial services.

![Cross Cloud Connections](https://github.com/microsoft/Federal-Business-Applications/blob/main/whitepapers/power-plat-d365-architecture/files/Slide5.PNG)
Image credit: [Steve Winward](https://github.com/microsoft/Federal-Business-Applications/tree/main/whitepapers/power-plat-d365-architecture)

Azure connectors in GCC require deliberate engineering to support cross cloud connections. The SQL Server connector has long offered this flexibility and an ongoing engineering effort is underway to enhance additional connectors. Engineering teams are working in partnership with field teams to prioritize these efforts. The top priority identified was the Azure Blob Storage connector, which now offers Microsoft Entra ID Integrated (Azure Commercial) as an option for Authentication.  

![Azure Blob Storage Connection](https://imgur.com/HF3KOPK.png)

### Prerequisite

To support governance of cross cloud connections, there is a new setting in the Power Platform Admin Center to allow such connections. This is a tenant-wide setting that can only be changed by a tenant administrator (Global Admin or Power Platform Admin).

![Connect to Azure Commercial](https://imgur.com/Owzhvyl.png)

### Going forward

We anticipate cross cloud flexibility for additional Azure connectors and will maintain this table as updates become available.

|Connector|Status|Date|
|-|-|-|
|Azure SQL (via SQL Server)|Available|Legacy|
|Azure Blob Storage|Available|June 2024|
|Azure Key Vault|Under Construction|~August, 2024|
|Azure Cosmos DB|Under Construction|~August, 2024|
|Azure Resource Manager|Nominated Priority|TBD|
|All other Azure connectors|Under Consideration|TBD|

### Interim Options

In the interim, the Azure service APIs called via HTTP or custom connector is always a possibility for a workaround. We have an example of this approach [here](../../demos/GCC-to-Commercial/). The example used happens to be obsolete as the Azure Blob Storage connector was our top priority for enhancement, but the patterns and techniques utilized remain valid.
