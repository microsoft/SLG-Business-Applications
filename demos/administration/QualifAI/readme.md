# QualifAI

## Entity Relationship Diagram
The custom tables and their relationship are defined below.

### Job Posting
This is the central anchor of the system representing an open requisition. It stores all the "front-door" information for the public-facing website, such as the job title, department, salary range, and the date the posting expires.
- **Title**: public job title 
- **Description Text**: full job description 
- **Status**: Draft, Active, Closed, Paused (option)

### Job Requirement
This is a junction table that maps specific Skills to a Job Posting. Its purpose is to define the "Success Profile" for a role. It allows a hiring manager to say, "To be successful in this Job Posting, you need these five specific skills."
- **Job Posting Lookup**: Lookup to `Job Posting` 
- **Skill Lookup**: Lookup to `Skill` 
- **Importance Weight**: Integer (1–10)
- **Minimum Years Required**: Integer

### Skill
This is your global library or "dictionary" of every possible competency an organization might care about (e.g., "Python," "Public Speaking," "Strategic Planning"). By keeping this as a standalone table, you ensure data consistency so that "Java" isn't accidentally entered as "Javascript" or "JAVA" elsewhere in the system. 
- **Name**: standardized competency name 
- **Description**: definition of the skill

### Candidate
This is the Identity table for the person applying. By separating this from the Application, you allow a single person to have a persistent profile in your system, enabling you to track their history if they apply for different roles over several years.
- **Full Name**: String
- **Email**: String (Unique) 
- **Resume** File (`.pdf` or `.docx`) 
- **LinkedIn URL**: String/URI 
- **Phone**: phone number of the candidate

### Application
This table represents the transaction of a candidate expressing interest in a job. It acts as a container for that specific journey, tracking which Candidate applied for which Job Posting and what their current status is (e.g., "Screening," "Interviewing," or "Rejected").
- **Job Posting**: lookup to the `Job Posting` being applied to
- **Candidate**: lookup to the `Candidate` that applied
- **Stage**: New, Screened, Interviewing, Rejected, Offered, Accepted

### Screening Question
This table acts as a template for the specific inquiries a hiring manager wants to make for a particular Job Posting. It defines the "what" and the "how", storing the question text and the expected format (e.g., boolean, multiple choice, or short text). It allows the system to gather structured data early in the funnel.
- **Job Posting**: Lookup to the `Job Posting` this question is for
- **Question Text**: prompt shown to candidate 

### Screening Answer
This is the data entry table where the candidate’s specific responses live. It maps a response back to both the Screening Question (to know what was asked) and the Application (to know who said it). This is the primary dataset your AI will likely scan first to determine if a candidate meets the "must-have" criteria. 
- **Application**: Lookup to the `Application` this answer is on
- **Question Lookup**: Lookup to the original `Screening Question` this is answering
- **Answer**: raw candidate input (their answer to the question), as text

### Application Evaluation
This is where your AI "Alignment Index" and summary logic live. It serves as the record for a specific assessment event, storing the high-level "All Up" score and the AI's qualitative reasoning for why the candidate is or isn't a fit for the application.
- **Application Lookup**: Lookup to the `Application` this review is for
- **Fit Score**: grade as to how close of a match the candidate is for the *overall* job posting, 0-100%
- **Justification**: general commentary on candidate fit for the job posting, supporting the score
- **Evaluated At**: Timestamp of when the evaluation was performed

### Skill Assessment
This is the most detailed table in your model, mapping the Application Evaluation back to individual Skills. Its purpose is to store the specific "score" the AI gave the candidate for each required skill, providing the evidence used to calculate the overall alignment score.
- **Evaluation Lookup**: Lookup to `Application Evaluation`
- **Skill Lookup**: Lookup to `Skill`
- **Score**: grade as to how well the candidate fulfilles a skill requirement for the job posting, 0-100%
- **Justification**: commentary supporting the score