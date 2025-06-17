# Health Plan Complaint Tracking
This solution captures complaints from the public, reviews the details of the complaint and works with the corresponding Health Plan to reconcile with the resident.

The workflow of the solution starts with the Resident who has been denied coverage for a specific procedure.  Via the Portal the Patient can self-report, can have an authorized representative report, or if the patient is a child, their Parent can report.  

This report will create a case for State Officials to track the Independent Medical Review process.  If that review requires information from the Health Plan, they can issue a RHPI (Request for Health Plan Information).  Health Plans can use the Portal to login to review the RHPI’s and provide a response.

This response can help determine whether health care services should be covered by the health plan.

[Click here](https://livesend.microsoft.com/i/XRTKvAT___Q5CygccMgK6iZyC0zGjVFm___ttK9KT7o4cZTTXmhRCItrfstEix___b9h2aGudbdQps2fw0T9dLkwPLUSSIGNOCoUw6aHsPD8VgbhEws4KwrVZKriyLAfWKG1JvtSyOFcZ) for a demonstration of this solution!

## Assets
The solution requires the pre-install of Dynamics 365 Customer Service and Power Pages solutions.

- [IMRProcess_1_0_0_2.ZIP](https://github.com/microsoft/SLG-Business-Applications/releases/download/30/IMRProcess_1_0_0_2.ZIP) - core solution file.
- [customer-provider-portal---engagementportal.ZIP](https://github.com/microsoft/SLG-Business-Applications/releases/download/30/customer-provider-portal---engagementportal.ZIP) - configured Customer Self-Service Portal that can be unzipped and uploaded via the PAC CLI.

## Credit
Solution made by [John Nelson](https://www.linkedin.com/in/jnelson-phx/).