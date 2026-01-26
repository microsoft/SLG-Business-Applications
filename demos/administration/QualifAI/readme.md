# QualifAI

## Entity Relationship Diagram
The custom tables and their relationship are defined below.

### Job Posting
This is the central anchor of the system representing an open requisition. It stores all the "front-door" information for the public-facing website, such as the job title, department, salary range, and the date the posting expires.
- **title:** String - public job title 
- **description_text:** Text - full job description 
- **status:** Enum (Draft, Active, Closed, Paused) 
- **department_id:** FK to department table

### Job Requirement
This is a junction table that maps specific Skills to a Job Posting. Its purpose is to define the "Success Profile" for a role. It allows a hiring manager to say, "To be successful in this Job Posting, you need these five specific skills."
- **job_posting_id (FK):** Links to a specific job 
- **skill_id (FK):** Links to the required skill
- **importance_weight:** Integer (1–10) 
- **min_years_required:** Integer

### Skill
This is your global library or "dictionary" of every possible competency an organization might care about (e.g., "Python," "Public Speaking," "Strategic Planning"). By keeping this as a standalone table, you ensure data consistency so that "Java" isn't accidentally entered as "Javascript" or "JAVA" elsewhere in the system.
- **id (PK):** Unique identifier (UUID or Integer) 
- **name:** String - standardized competency name (e.g., "Python", "Conflict Resolution") 
- **category:** String/Enum - groups skills (e.g., "Technical", "Soft Skill", "Compliance") 
- **description:** Text - clear definition of the skill

### Candidate
This is the Identity table for the person applying. By separating this from the Application, you allow a single person to have a persistent profile in your system, enabling you to track their history if they apply for different roles over several years.
- **id (PK):** Unique identifier 
- **full_name:** String 
- **email:** String (Unique) 
- **resume_url:** String/URI 
- **linkedin_url:** String/URI 
- **phone:** String

### Application
This table represents the transaction of a candidate expressing interest in a job. It acts as a container for that specific journey, tracking which Candidate applied for which Job Posting and what their current status is (e.g., "Screening," "Interviewing," or "Rejected").
- **id (PK):** Unique identifier 
- **application_id (FK):** Links to application 
- **alignment_index:** Decimal (0.00–1.00) - AI fit score 
- **ai_summary:** Text - narrative explanation 
- **evaluated_at:** Timestamp

### Screening Question
This table acts as a template for the specific inquiries a hiring manager wants to make for a particular Job Posting. It defines the "what" and the "how", storing the question text and the expected format (e.g., boolean, multiple choice, or short text). It allows the system to gather structured data early in the funnel.
- **id (PK):** Unique identifier 
- **job_posting_id (FK):** Ties question to job 
- **question_text:** Text - prompt shown to candidate 
- **input_type:** Enum (Boolean, Number, Multiple Choice, Long Text) 
- **is_knockout:** Boolean - can auto-reject based on answer

### Screening Answer
This is the data entry table where the candidate’s specific responses live. It maps a response back to both the Screening Question (to know what was asked) and the Application (to know who said it). This is the primary dataset your AI will likely scan first to determine if a candidate meets the "must-have" criteria.
- **id (PK):** Unique identifier 
- **application_id (FK):** Ties answer to application 
- **question_id (FK):** Ties answer to question 
- **answer_value:** Text/JSON - raw candidate input

### Application Evaluation
This is where your AI "Alignment Index" and summary logic live. It serves as the record for a specific assessment event, storing the high-level "All Up" score and the AI's qualitative reasoning for why the candidate is or isn't a fit for the application.
- **id (PK):** Unique identifier 
- **application_id (FK):** Links to application 
- **alignment_index:** Decimal (0.00–1.00) - AI fit score 
- **ai_summary:** Text - narrative explanation 
- **evaluated_at:** Timestamp

### Skill Assessment
This is the most detailed table in your model, mapping the Application Evaluation back to individual Skills. Its purpose is to store the specific "score" the AI gave the candidate for each required skill, providing the evidence used to calculate the overall alignment score.
- **id (PK):** Unique identifier 
- **evaluation_id (FK):** Links to evaluation 
- **skill_id (FK):** Skill being graded 
- **score:** Integer (1–100) 
- **justification:** Text - evidence supporting the score