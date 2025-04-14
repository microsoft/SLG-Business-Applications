# Inventory Management
The Contoso Health Lab Inventory Management System is a cutting-edge solution built on the Microsoft Power Platform, utilizing Dataverse as its centralized, secure, and scalable data backbone. Designed to streamline inventory management for natural disaster responses and infectious disease outbreaks, this system provides an intuitive and efficient way to track essential supplies and deploy critical resources when needed most.

The system is structured around two integrated applications:

Canvas App for Field Personnel: Tailored for first responders and field staff, this user-friendly mobile application provides real-time inventory management and reporting capabilities. Accessible via smartphones and tablets running the Power Platform mobile app, field personnel can quickly scan items, update quantities, and log new entries using built-in barcode scanning capabilities. This streamlined, intuitive experience empowers responders to efficiently manage supplies from the field, even in remote or high-pressure environments.

Model-Driven App for Back-Office Staff: Built to support administrative and logistical personnel, this application offers a comprehensive view of inventory levels, kits, and replenishment needs. Leveraging Dataverse’s robust relational data model, back-office users can configure low-item warning thresholds, monitor usage trends, and generate detailed reports. The model-driven app provides enhanced visibility and control over inventory, enabling proactive decision-making and seamless coordination across teams.

At its core, the system supports the creation and management of standardized kits, which are essential for rapid response scenarios. For instance, an Earthquake Quick-Response Kit might include water, bandages, antiseptic wipes, flashlights, batteries, and basic medical supplies. Kits are designed to be easily customized, allowing administrators to adjust contents based on evolving requirements or specific emergency scenarios.

The Power Platform’s integration capabilities further enhance the system’s flexibility. It interfaces seamlessly with major barcode readers and supports mobile scanning via smartphones, ensuring compatibility with existing hardware and minimizing operational friction. Additionally, robust automation features, including Power Automate, streamline notification processes—triggering alerts when inventory falls below critical thresholds or when kits require replenishment.

By harnessing the full power of Microsoft Power Platform, this inventory management system delivers a cohesive and scalable solution tailored to the unique needs of public health agencies. With Dataverse providing a secure and scalable data structure, the system ensures that emergency response teams remain prepared, responsive, and coordinated, even under the most demanding circumstances.

## Solution Files
- [InventoryManagement_1_0_0_1](https://github.com/microsoft/SLG-Business-Applications/releases/download/24/InventoryManagement_1_0_0_1.zip) - no Copilot Studio Agent; no modified Canvas App for Copilot Studio Agent.
- [InventoryManagement_1_0_0_2.zip](https://github.com/microsoft/SLG-Business-Applications/releases/download/24/InventoryManagement_1_0_0_2.zip) - contains the Copilot Studio Agent and the modified Canvas App. At the time of writing this, that functionality was not quite done in GCC.

## Sample Data
The **schema** can be found [here](./inventory-mgt-schema.xml), while the sample data can be downloaded [here](https://github.com/microsoft/SLG-Business-Applications/releases/download/24/inventory-management-data.zip).

## Install Instructions
*Set up instructions to come*

## Credits
Made by [Patrick O'Gorman](https://www.linkedin.com/in/patrick-ogorman/).