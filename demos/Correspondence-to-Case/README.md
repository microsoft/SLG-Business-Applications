# 📨 Correspondence to Case – Dynamics 365 Case Automation with AI Builder

This solution provides a complete framework for **automatically transforming inbound citizen correspondence into actionable cases** within Dynamics 365 Customer Service or Contact Center. Built specifically for the needs of **State and Local Government (SLG)** agencies, this low-code solution leverages **Power Automate**, **AI Builder**, and **Dataverse** to streamline case intake, classification, and triage—regardless of communication channel.


## Architecture Diagram

![CorrespondenceToCaseArchitecture](https://github.com/user-attachments/assets/0c091ee0-6466-4308-bdcd-fd9cdcb1a49c)



## 🧠 How It Works

1. **Multiple Case Origin Channels**  
   Citizens can submit inquiries via:
   - Email  
   - Web forms  
   - Scanned letters, PDFs, or paper forms  
   - Fax or fillable forms  
   - Live chat or phone (transcription)  
![CustomPage](https://github.com/user-attachments/assets/cd35decf-c61e-497d-bea3-4afc407663c1)

2. **AI-Powered Parsing**  
   A Power Automate flow triggers a **Custom Prompt** model in **AI Builder** to extract:
   - Customer name  
   - Case number or topic  
   - Confidence score  
   - Any relevant metadata
     
![AIBuilderCustomPrompt](https://github.com/user-attachments/assets/05bfde10-b3ba-41e3-b2d8-b81440cede82)

3. **Case Creation**  
   - Extracted data is used to **automatically create a new case** in Dynamics 365.
   - Based on the model’s confidence, the case can either be auto-routed or flagged for agent verification.

![PowerAutomateFlow](https://github.com/user-attachments/assets/bdb19019-7d16-4451-b395-3a59c2faf63f)

---

## 🧑‍💼 Agent Workflow

Cases created via the Correspondence Processor appear directly in the **Customer Service Workspace** for agent triage and response:

- Filter by AI Confidence Level  
- Review generated case summaries  
- Work from prefilled data or make corrections  
- Proceed to resolution and closure

![CustomerServiceCases](https://github.com/user-attachments/assets/550fd824-9e46-4437-a9e9-09ad7e40eb0e)

---

## 🔧 What’s Included

- Power Automate flow for intake, prompt execution, and case creation  
- AI Builder model with a customizable prompt  
- Custom Canvas page embedded in Dynamics 365  
- Sample Dataverse tables and configuration  

[CorrespondencetoCase_1_0_0_1.zip](https://github.com/user-attachments/files/20110219/CorrespondencetoCase_1_0_0_1.zip)

---

## 💡 Ideal For:

- SLG departments receiving high volumes of citizen inquiries  
- Agencies seeking to reduce manual data entry and improve response time  
- Compliance-driven organizations needing case audit trails and automation  
- Teams experimenting with AI Builder + Copilot integration

---

## 📌 Requirements

- Microsoft Power Platform environment  
- AI Builder credits (for custom prompt models)  
- Dynamics 365 Customer Service or Contact Center license  
- Power Automate Premium (or per-flow plan)

---

## 📽️ Videos

**Creating a Custom Prompt in AI Builder** 


https://github.com/user-attachments/assets/2bd533c1-23be-4562-8f79-146179cb1ca3


**Configuring a Power Automate Flow which calls Custom Prompt and Dynamics 365** 


https://github.com/user-attachments/assets/176328eb-9e9c-4015-abef-d2ca4f0e1572


**Processing Correspondence to Cases in Dynamics 365** 


https://github.com/user-attachments/assets/0abb8580-8e2c-48a9-8dc4-3bc1f39c0977


---


