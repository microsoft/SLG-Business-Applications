# Using AI to Automate Foster Home Inspections
Social workers play a critical role in ensuring that foster homes are safe and suitable for foster child placement. Traditionally, this involves meticulous inspections and assessments of potential foster parent applicants' homes to guarantee a nurturing and secure environment. However, this process can be cumbersome and time-consuming, often relying on extensive manual assessments with paper and pen.

We can leverage an AI-driven approach to streamline and enhance this process. By allowing social workers to capture photos of the home using a mobile app, an advanced multimodal AI model can automatically assess the images against a pre-defined set of criteria. This technology evaluates the home's preparedness level for foster child placement and generates a readiness report, saving valuable time and resources for social workers.

[![video_image](https://i.imgur.com/QTh0TbT.png)](https://youtu.be/gAyiKXlnkYo)

## Benefits
- **Efficiency**: Automating the assessment process reduces the time required for social workers to complete home inspections, allowing them to focus on other critical tasks.
- **Consistency**: The AI model applies standard criteria consistently, ensuring uniform evaluations across different inspections.
- **Accessibility**: The mobile app makes it easy for social workers to document and assess homes on-the-go, without the need for cumbersome paper-based methods.
- **Reporting**: The generated report provides detailed insights and recommendations, aiding social workers in making informed decisions.

## How is this done?
Inspired by [EVVIE](https://aka.ms/EVVIE), a system that uses AI to assess damage to vehicles, this system uses advanced multimodal artificial intelligence (AI) to assess how ready a home is to receive a foster child.

A [Power Apps](https://learn.microsoft.com/en-us/power-apps/powerapps-overview) canvas app is used as the interface that the social worker interacts with on their smartphone. The app allows for the case worker to capture photos throughout the home. With a single click of a button, these photos are passed to a multimodal AI model, deployed in Azure OpenAI. This model assesses the photos against a standard criteria (see below). The model performs the assessment against the criteria and outputs its rating in a specific format. This output is then displayed to the case worker in the Power Apps canvas app where it saved and can also be sent to the foster parent applicant.

Below, you'll find the three critical elements that make this possible: explicit instructions on to the AI model that define its task (prompt engineering), the photos of the home, and the criteria by which the photos should be graded against. All wrapped within an easy-to-use and intuitive mobile app!

![ingredients](https://i.imgur.com/h0zImki.png)

## Credit
Designed by [Tim Hanewich](https://github.com/TimHanewich).