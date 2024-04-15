# Dataverse Web API
[This Postman collection](./Dataverse%20Web%20API.postman_collection.json) contains example requests for transacting with the Dataverse Web API:
- Authorization
- Listing tables in an environment
- Reading all contact records
- Creating a contact record
- Updating a contact record
- Deleting a contact record
- Performing a complex query on the contacts table, with filtering, ordering, and more
- Accessing metadata for the contact table
- Accessing metadata for option sets in an environment
- Downloading an image stored as a column in a record
- Download a file stored as a column in a record

## Steps to use this postman collection
1. [Download and install Postman](https://www.postman.com/downloads/)
2. In Postman, click on *Collections* on the left pane. Click on *import* and select the [Dataverse Postman Collection](./Dataverse%20Web%20API.postman_collection.json) from this repo.
3. Click on *Environments* on the left pane. Create a new **environment** in Postman with the following variables, plugging in your information in the *Current Value* column.
    1. *username* - your Azure AD username
    2. *password* - your Azure AD password
    3. *resource* - the URL of the Dataverse environment you would like to access
    4. *token* - After plugging in the above three variables, use the **Authorize** function to request an Access token, labeled `access_token`. Copy & Paste this value (starts with "ey") into a variable called *token* in your new environment
4. On the top right part of Postman, click on the environment dropdown and select the new environment that you made

Each request in the collection is designed to dynamically reference these variables. After plugging in the *token* variable in your new environment, you are now able to run any request found in this collection.

## Before using
All of the example requests from this collection use the out of the box `contact` table, except for the final two: *Download image* and *Download file*. For the sake of this example kit, a field called *Profile Picture* (logical name "crfce_profilepicture") and a field called *Application* (logical name "crfce_application") was added so downloading of an image and file could be demonstrated. If you do not add these fields to **your** contact table (and logical names match), you will not be able to demonstrate these two requests.