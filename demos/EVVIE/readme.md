# E.V.V.I.E. - **E**nterprise **V**isual **V**ehicle **I**nspection **E**ngine
EVVIE, short for “Enterprise Visual Vehicle Inspection Engine”, is an AI-powered vehicle inspection platform that streamlines the inspection process for state and local government fleets, using artificial intelligence to analyze images of vehicles and detect damage or issues, enabling agencies to improve accuracy, reduce paperwork, and optimize maintenance management.

Click the image below for a demonstration of E.V.V.I.E.

[![demo video](https://i.imgur.com/iNToam3.png)](https://youtu.be/Kw98NkFkoXI)

## How does EVVIE Work?
EVVIE uses a unique blend of Microsoft Power Platform and Microsoft Azure resources to automate the vehicle inspection process using advanced, state-of-the-art AI models. EVVIE's internal process is described below, at a high level:

A **Power Apps** (canvas) app used by the user and serves as the interface to collect photos of damage to a vehicle as part of a vehicle inspection.

The Power App leverages a **custom connector** (API integration) to provide the captured images to a custom API, a .NET-based **Azure Function**.

The Azure Function receives and parses the HTTP API call (images are encoded as a `base64` string) from the Power App, takes these photos, and interfaces with a multimodal LLM in **Azure OpenAI Service**. The multimodal LLM is instructed to review the photos and classify the damage into three distrinct fields: 1) area of car, 2) severity level (1-5), and 3) general description of the damage.

The **Power Apps** inspection app receives this response back from the **Azure Function**, presenting the AI-created damage assessment to the user, where the user has the option of *accepting*, *modifying*, or *rejecting* the assessment altogether. After finalizing the assessment, they then submit this assessment for the given vehicle where it is securely stored in **Dataverse**.

Another **Power Apps** app (model-driven with custom pages) allows for administrators to review this vehicle inspection data.

## Architecture
EVVIE's architecture is a unique blend of Power Platform (low-code) and Azure (pro-code) resources, coming together to deliver a single unified AI-enabled service to users. The architecture is depicted below, but can be accessed in PowerPoint deck form [here](./architecture.pptx).

![architecture](https://i.imgur.com/uT6N82Y.jpeg)

## E.V.V.I.E. Source Code
The source code of E.V.V.I.E. is provided in the `src` folder as follows:
- [core](./src/core/) - this contains a C# console application that is used essentially as a library of functions and capabilities that E.V.V.I.E. relies on for communicating with the Azure OpenAI service. This allows E.V.V.I.E. to reach out to the Azure OpenAI service to do things like identify vehicles via their license plate and assess damage to vehicles.
- [api](./src/api/) - this contains the code to a **v4**, **.NET 8.0-based** Azure Function, written in C#, that exposes two endpoints that the E.V.V.I.E. interface, built in Power Apps, can call to. Those endpoints are `/plate`, reading a license plate number from a single provided image in *base64* format, and `/inspect`, assessing the damage to a vehicle based on one or multiple provided images of the damage to the car in *base64* format.

## One-Pager
A "one-pager" flyer exists for EVVIE:

![one-pager](https://i.imgur.com/Y3dUY0M.jpeg)

The one-pager can be downloaded in various formats below:
- [As a PowerPoint deck](./one-pager/one-pager.pptx)
- [In PDF Form](./one-pager/one-pager.pdf)