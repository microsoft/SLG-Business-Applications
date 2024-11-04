# Dataverse Performance Testing

## Tests
Tests will run for at least 60 minutes, or as long as a single programmatic authentication into Dataverse lasts (generally ~70 minutes). Continuous uploads/upserts/updates will be POSTed to the Dataverse web API, in various formats, and this will continue with no delay/wait until the access token expires (and thus an error is given). By comparing the ratio of records impacted vs. time, we can determine which methods were fastest.

We will be comparing the performance of the following methods:
- 1-by-1 uploads (one record be HTTP POST)
- 1-by-1 uploads (one record per HTTP POST) on [Elastic Tables](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/elastic-tables)
- [CreateMultiple](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/bulk-operations?tabs=webapi#createmultiple)
    - Batches of 25
    - Batches of 100
    - Batches of 500
    - Batches of 1000
- [CreateMulitple](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/bulk-operations?tabs=webapi#createmultiple) **with** [Elastic Tables](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/elastic-tables)
    - Batches of 25
    - Batches of 100
    - Batches of 500
    - Batches of 1000