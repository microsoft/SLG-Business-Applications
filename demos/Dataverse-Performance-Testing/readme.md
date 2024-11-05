# Dataverse Performance Testing

## Test conditions
- In a GCC environment
- In a Sandbox environment

## Schema
Tests will be performed on a custom table, `Animal`, in a ficticious farm management use case. 

The `Animal` table will have the following schema:
- ID, an autonumber(Primary column)
- Name (string)
- Species (option set)
- Date of Birth (date)
- Weight, in pounds (whole number, integer)
- Daily Feed Intake, in pounds (decimal number)

The `Animal Species` option set will have the following options
- Cow
- Pig
- Chicken
- Sheep
- Horse
- Goat

You can download a **solution** with the option set, table, and a simple model-driven app [here](https://github.com/microsoft/SLG-Business-Applications/releases/download/16/DataversePerformanceTesting_1_0_0_1.zip).

## Tests
Tests will run for at least 60 minutes, or as long as a single programmatic authentication into Dataverse lasts (generally ~70 minutes). Continuous uploads/upserts/updates will be POSTed to the Dataverse web API, in various formats, and this will continue with no delay/wait until the access token expires (and thus an error is given). By comparing the ratio of records impacted vs. time, we can determine which methods were fastest.

We will be comparing the performance of the following methods:
- **Test 1**: One record per HTTP request, each request one-by-one
- **Test 2**: One record per HTTP reqquest, but HTTP requests sent concurrently in groups of 50.
- **Test 3**: Multiple records per HTTP request using the [CreateMultiple](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/bulk-operations?tabs=webapi#createmultiple) service, each HTTP request one-by-one.
    - Batches of 25 per HTTP call
    - Batches of 100 per HTTP call
    - Batches of 500 per HTTP call
    - Batches of 1000 per HTTP call
- **Test 4**: Multiple records per HTTP requests using the [CreateMultiple](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/bulk-operations?tabs=webapi#createmultiple) service, but HTTP requests sent concurrently in groups of 50.
    - Batches of 25 per HTTP call
    - Batches of 100 per HTTP call
    - Batches of 500 per HTTP call
    - Batches of 1000 per HTTP call
- **Test 5**: Multiple records per HTTP request, using the [CreateMultiple](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/bulk-operations?tabs=webapi#createmultiple) service, each HTTP request one-by-one, but against an [Elastic Table](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/elastic-tables)
    - Batches of 25 per HTTP call
    - Batches of 100 per HTTP call
    - Batches of 500 per HTTP call
    - Batches of 1000 per HTTP call