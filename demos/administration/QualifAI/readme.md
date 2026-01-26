# QualifAI

## Entity Relationship Diagram
The custom tables and their relationship are defined below.

### Job Posting
This is the central anchor of the system representing an open requisition. It stores all the "front-door" information for the public-facing website, such as the job title, department, salary range, and the date the posting expires.
- **Title:** String – public job title 
- **Description Text:** Text – full job description 
- **Status:** Enum (Draft, Active, Closed, Paused) 
- **Department Lookup:** Lookup to Department table

### Job Requirement
This is a junction table that maps specific Skills to a Job Posting. Its purpose is to define the "Success Profile" for a role. It allows a hiring manager to say, "To be successful in this Job Posting, you need these five specific skills."
- **Job Posting Lookup:** Lookup to Job Posting 
- **Skill Lookup:** Lookup to Skill 
- **Importance Weight:** Integer (1–10) 
- **Minimum Years Required:** Integer

### Skill
This is your global library or "dictionary" of every possible competency an organization might care about (e.g., "Python," "Public Speaking," "Strategic Planning"). By keeping this as a standalone table, you ensure data consistency so that "Java" isn't accidentally entered as "Javascript" or "JAVA" elsewhere in the system.
- **Job Posting Lookup:** Lookup to Job Posting 
- **Skill Lookup:** Lookup to Skill 
- **Importance Weight:** Integer (1–10) 
- **Minimum Years Required:** Integer

### Candidate
This is the Identity table for the person applying. By separating this from the Application, you allow a single person to have a persistent profile in your system, enabling you to track their history if they apply for different roles over several years.
- **ID (PK):** Unique identifier 
- **Full Name:** String 
- **Email:** String (Unique) 
- **Resume URL:** String/URI 
- **LinkedIn URL:** String/URI 
- **Phone:** String

### Application
This table represents the transaction of a candidate expressing interest in a job. It acts as a container for that specific journey, tracking which Candidate applied for which Job Posting and what their current status is (e.g., "Screening," "Interviewing," or "Rejected").
- **ID (PK):** Unique identifier 
- **Application Lookup:** Lookup to Application 
- **Alignment Index:** Decimal (0.00–1.00) – AI fit score 
- **AI Summary:** Text – narrative explanation 
- **Evaluated At:** Timestamp

### Screening Question
This table acts as a template for the specific inquiries a hiring manager wants to make for a particular Job Posting. It defines the "what" and the "how", storing the question text and the expected format (e.g., boolean, multiple choice, or short text). It allows the system to gather structured data early in the funnel.
- **ID (PK):** Unique identifier 
- **Job Posting Lookup:** Lookup to Job Posting 
- **Question Text:** Text – prompt shown to candidate 
- **Input Type:** Enum (Boolean, Number, Multiple Choice, Long Text) 
- **Is Knockout:** Boolean – auto‑reject flag

### Screening Answer
This is the data entry table where the candidate’s specific responses live. It maps a response back to both the Screening Question (to know what was asked) and the Application (to know who said it). This is the primary dataset your AI will likely scan first to determine if a candidate meets the "must-have" criteria.
- **ID (PK):** Unique identifier 
- **Application Lookup:** Lookup to Application 
- **Question Lookup:** Lookup to Screening Question 
- **Answer Value:** Text/JSON – raw candidate input

### Application Evaluation
This is where your AI "Alignment Index" and summary logic live. It serves as the record for a specific assessment event, storing the high-level "All Up" score and the AI's qualitative reasoning for why the candidate is or isn't a fit for the application.
- **ID (PK):** Unique identifier 
- **Application Lookup:** Lookup to Application 
- **Alignment Index:** Decimal (0.00–1.00) 
- **AI Summary:** Text 
- **Evaluated At:** Timestamp

### Skill Assessment
This is the most detailed table in your model, mapping the Application Evaluation back to individual Skills. Its purpose is to store the specific "score" the AI gave the candidate for each required skill, providing the evidence used to calculate the overall alignment score.
- **ID (PK):** Unique identifier 
- **Evaluation Lookup:** Lookup to Application Evaluation 
- **Skill Lookup:** Lookup to Skill 
- **Score:** Integer (1–100) 
- **Justification:** Text – evidence supporting the score