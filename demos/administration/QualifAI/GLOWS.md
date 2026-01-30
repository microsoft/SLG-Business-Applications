# HR Recruiting: A Data Model for Hiring and Candidate Management
The HR Recruiting module provides a standardized, extensible structure for managing the modern government recruitment lifecycle. Moving beyond basic applicant tracking, this model is designed to support Competency-Based Hiring. It allows agencies to define the specific "Success Profile" of a role and measure candidates against those requirements with precision and transparency.

## How the Model Works
At the heart of the system is the **Job Posting** table, the central anchor for a requisition. Rather than relying on static text alone, this model utilizes a **Skill** library and **Job Requirements** junction. This allows hiring managers to "tag" a posting with specific competencies (i.e. "Data Analysis" or "Project Management"), assigning importance weights and minimum experience levels to each.

**Candidates** are managed via the industry-standard Contact table, ensuring a persistent identity for applicants across their entire career. When a candidate applies, an **Application record** is created, linking the person to the posting and tracking their journey through the hiring stages—from initial screening to the final offer.

The model also introduces a structured **Screening** and **Evaluation** layer. Agencies can define specific **Screening Questions** per job, with **Screening Answers** captured directly on the application. Once a candidate is under review, the Application Evaluation and Skill Assessment tables allow for granular scoring. Instead of a single "gut-feeling" score, evaluators can provide evidence-based grades for every individual skill required for the position.

## Scalability and Growth
This module is built for flexibility. A small team can use the core `Job Posting` and `Application` tables for simple tracking. Larger, more mature agencies can activate the full `Evaluation` and `Skill Assessment` suite to perform deep talent evaluation and analytics. By unifying job requirements, applicant data, and qualitative assessments, the HR Recruiting module transforms hiring from a manual exercise into a strategic, data-backed process.

## Entity Relationship Diagram
![erd](https://i.imgur.com/cqRfVK1.png)

This package's custom tables and their relationships are further described below:

### Job Posting
This is the central anchor of the system representing an open requisition. It stores all the "front-door" information for a public-facing job posting, such as the job title, salary range, and the date the posting expires.
- **Title**: public job title 
- **Description Text**: full job description 
- **Status**: Draft, Active, Closed, Paused (option)
- **Salary Low End**: the low-end guidance for the expected salary
- **Salary High End**: the high-end guidance for the expected salary
- **Expires**: the date the job posting will expire 

### Skill
This is the global library or "dictionary" of competencies an organization might care about when hiring (e.g., "Python," "Public Speaking," "Strategic Planning", "People Management").
- **Name**: standardized competency name
- **Description**: definition of the skill

### Job Requirement
This is a junction table that maps specific a specific `Skill` to a `Job Posting`. Its purpose is to define the "Success Profile" for a role (the skills a candidate must possess for a particular `Job Posting`). It allows a hiring manager to say, "To be successful in this Job Posting, you need these five specific skills."
- **For Job Posting**: Lookup to `Job Posting` 
- **Required Skill**: Lookup to `Skill` 
- **Importance Weight**: weight of how important this particular skill is, 1-10
- **Minimum Years Required**: how many years of this skill the candidate must possess

### Candidate
Leveraging Dataverse's OOTB *Contact* table from the common data model, this served as the identity table for the person applying. Each candidate has a persistent profile, enabling the organization to track their history if they apply for different roles over several years.
- **LinkedIn Profile**: Link to their LinkedIn profile (URL) 
- *All other fields are fulfilled via OOTB columns provided in the Contact table*

### Screening Question
This table stores the specific inquiries (individual questions) a hiring manager wants to have in a particular Job Posting.
- **On Job Posting**: Lookup to the `Job Posting` this question is for
- **Question Text**: prompt (question) shown to candidate

### Application
A candidate's application to a specific `Job Posting`, containing all relevant information they have submitted for consideration.
- **Applied To**: lookup to the `Job Posting` being applied to
- **Applying Candidate**: lookup to the `Candidate` that applied
- **Stage**: New, Screened, Interviewing, Rejected, Offered, Accepted
- **Candidate Resume** file (`.pdf` or `.docx`) 

### Screening Answer
Stores each of the candidate's specific responses to each `Screening Question`. It maps a response back to both the Screening Question (to know what was asked) and the Application (to know who said it). 
- **On Application**: Lookup to the `Application` this answer is on
- **Question**: Lookup to the original `Screening Question` this is answering
- **Answer**: raw candidate input (their answer to the question), as text

### Application Evaluation
Stores an evaluation of a candidate's `Application`, storing the high-level "All Up" score and qualitative reasoning for why the candidate is or isn't a fit for the job.
- **Evaluated Application**: Lookup to the `Application` this review is for
- **Fit Score**: grade as to how close of a match the candidate is for the *overall* job posting (whole number, 0-100)
- **Justification**: general commentary on candidate fit for the job posting, supporting the score
- **Evaluated Performed**: Timestamp of when the evaluation was performed

### Skill Assessment
Stores specific evaluation scores given to the candidate for *each* required `Job Requirement` skill, providing the evidence used to calculate the candidate's overall alignment score.
- **Part of Evaluation**: Lookup to `Application Evaluation` this assessment is a part of.
- **Evaluated Skill**: Lookup to the `Skill` that was evaluate
- **Score**: grade as to how well the candidate fulfilles a skill requirement for the job posting (whole number, 0-100)
- **Justification**: commentary supporting the score