# QualifAI: Intelligent Talent Acquisition
**QualifAI** is an end-to-end AI-driven recruitment system built on the Microsoft Power Platform. Designed to bridge the gap between high-volume job postings and high-quality candidate selection, QualifAI leverages advanced AI to automate the heavy lifting of the initial evaluation process.

- **Internal Operations (Power App)**: Staff use a robust Model-Driven app to architect job postings, define custom screening questions, and select specific "Hireable Skills" from a global library to build a weighted "Success Profile" for every role.
- **External Talent Portal (Power Pages)**: A public-facing portal where candidates can create secure accounts, browse open requisitions, submit structured applications, and track their real-time status in the hiring pipeline.
- **Evaluation AI (Microsoft Foundry)**: AI performs initial candidate application assessments. It evaluates candidates not just on keywords, but on their specific alignment with weighted job requirements and screening responses.

## Entity Relationship Diagram
The custom tables and their relationships are further described below.

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
- **Job Posting Lookup**: Lookup to `Job Posting` 
- **Skill Lookup**: Lookup to `Skill` 
- **Importance Weight**: weight of how important this particular skill is, 1-10
- **Minimum Years Required**: how many years of this skill the candidate must possess

### Candidate
This is the Identity table for the person applying. Each candidate has a persistent profile, enabling the organization to track their history if they apply for different roles over several years.
- **Full Name**: candidate's full name
- **Email**: candidate's email 
- **Resume** file (`.pdf` or `.docx`) 
- **LinkedIn URL**: String/URI 
- **Phone**: phone number of the candidate

### Screening Question
This table stores the specific inquiries (individual questions) a hiring manager wants to have in a particular Job Posting.
- **Job Posting**: Lookup to the `Job Posting` this question is for
- **Question Text**: prompt (question) shown to candidate

### Application
A candidate's application to a specific `Job Posting`, containing all relevant information they have submitted for consideration.
- **Job Posting**: lookup to the `Job Posting` being applied to
- **Candidate**: lookup to the `Candidate` that applied
- **Stage**: New, Screened, Interviewing, Rejected, Offered, Accepted

### Screening Answer
Stores each of the candidate's specific responses to each `Screening Question`. It maps a response back to both the Screening Question (to know what was asked) and the Application (to know who said it). 
- **Application**: Lookup to the `Application` this answer is on
- **Question**: Lookup to the original `Screening Question` this is answering
- **Answer**: raw candidate input (their answer to the question), as text

### Application Evaluation
Stores an evaluation of a candidate's `Application`, storing the high-level "All Up" score and qualitative reasoning for why the candidate is or isn't a fit for the job.
- **Application Lookup**: Lookup to the `Application` this review is for
- **Fit Score**: grade as to how close of a match the candidate is for the *overall* job posting, 0-100%
- **Justification**: general commentary on candidate fit for the job posting, supporting the score
- **Evaluated At**: Timestamp of when the evaluation was performed

### Skill Assessment
Stores specific evaluation scores given to the candidate for *each* required `Job Requirement` skill, providing the evidence used to calculate the candidate's overall alignment score.
- **Evaluation Lookup**: Lookup to `Application Evaluation`
- **Skill Lookup**: Lookup to `Skill`
- **Score**: grade as to how well the candidate fulfilles a skill requirement for the job posting, 0-100%
- **Justification**: commentary supporting the score