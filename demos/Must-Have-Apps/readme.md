# Power Platform Must-Have-Apps

## Inspection Start Kit Framework
Download the solution (.zip) file [here](https://github.com/TimHanewich/Power-Platform-Assets/releases/download/20/Inspections_1_0_0_1.zip).

For demo of solution, skip to 11:20 in [this video](https://youtu.be/7pA6yEFDmOY?t=680).

For demo of install and basic configuration, see [this video](https://youtu.be/r8H1tqzAZG8).

## HR Onboarding/Offboarding Start Kit Framework
Download the solution (.zip) file [here](https://github.com/TimHanewich/Power-Platform-Assets/releases/download/20/HRChecklistManagement_1_0_0_1.zip).

For demo of solution, skip to 34:00 in [this video](https://youtu.be/7pA6yEFDmOY?t=2040).

For demo of install and basic configuration, see [this video](https://youtu.be/Ao3YfQ0E8Cc).

Notes for what's covered in the videos above:

### Install Guidance
- Import solution
- Get your Environment ID from PPAC and provide as Environment Variable during import
- After import, go to Solution and Publish all customizations
- Turn On the Deactivate AAD Account Flow (this Flow won't actually deactivate accounts; see Note below)
- Run MDA and create some sample data: (if you can't play the app it's because you didn't publish) 
    - Package Type 
        - The envisioned options are Onboarding, Offboarding, Transition, etc.  Package type can be used in downstream logic but starter kit doesn’t use it in any consequential manner.
    - Requirement (Task) Template
    - Package Template 
        - Use a parent/child hierarchy to streamline setup/maintenance.  For example, if a Top Parent Offboarding template is created, and all templates are parented/grand parented to this template, any tasks added to the Top Parent will be assumed by all child checklists.
        - Add Requirement Templates to the Template using the subgrid

### Three Security Roles Have Been Created
- Checklist User - Can run the canvas app and update Task status; read only access to custom tables
- Checklist Creator - Can run the canvas app, update Task status and create new checklists; read only access to custom tables plus the ability to create Task Packages (i.e. create new checklists)
- Checklist Admin - Full access to all custom tables.  Can maintain templates and work with checklists.  The HR Checklist Management model driven app should be shared with this Security Role.

### Additional Notes
- The "New Checklist" button in the canvas app will only show for users who have been assigned the System Administrator, Checklist Admin, or Checklist Creator security roles.  At present, this must be a direct role assignment.  The canvas app will not recognize role inheritance through a Team.
- The People Picker screen pulls a list of Users from the Power Platform environment.  Checklists can only be created for folks who are a user in the environment.
- Tasks cannot be assigned to people who don't have sufficient permissions.  Suggested approach for ensuring this requirement is met: 
    - Stamp the environment with an Entra ID Security Group in PPAC
    - Create a Team for the same SG
    - Assign the Checklist User role to the Team - now all users in the environment will have minimum permissions
- Only open Tasks show in the Canvas App.
- Automated Tasks should have a corresponding Flow assigned.
- Automated Tasks do not show in the Canvas App. But if a task is marked is Automated but a Flow has not been assigned, it'll be assigned to the employee's manager.
- The solution contains a "Deactive AAD Account" flow.  
    - This Flow will not actually deactivate the user account; in fact, it doesn't connect to AAD at all as such an action will require elevated permissions. 
    - The Flow does act as a template showing how other data driven Flows should be constructed.  Notice how the Flow gets its own ID and uses that to determine which Tasks it should automate.
    - The Flow comes with a Manual trigger.  At some point the trigger should be changed to run on a schedule.

## Contributors
*Solutions made by [Doug Bell](https://www.linkedin.com/in/doug-bell-56090341/), [Doug Furney](https://www.linkedin.com/in/dougfurney/), [John Parker](https://www.linkedin.com/in/corporalparker/), [Dan Cox](https://www.linkedin.com/in/ideasoftware/), [Richa Sharma](https://www.linkedin.com/in/richa-t-sharma/), [David Pritchett](https://www.linkedin.com/in/david-pritchett-710a1a/), and [Kurt Reynolds](https://www.linkedin.com/in/kurt-reynolds-24259413/).*