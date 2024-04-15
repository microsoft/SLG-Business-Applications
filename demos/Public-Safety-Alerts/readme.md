# Webhook Service Example: Public Safety Alerts
![public safety alert](https://i.imgur.com/vS34Ep5.png)

This is a webhook service that serves to demonstrate the use of Power Platform's custom connector's actions & triggers against a fictitious public safety alert system.

## Completed Resources
- The Swagger definition for the completed connector can be found [here](./Public-Safety-Alerts.swagger.json). 
    - To import this definition as a custom connector and start using it right away in Power Apps and Power Automate, simply navigate to **Custom Connectors** in Power Automate, click on **New custom connector** at the top, and select **Import an OpenAPI file**, and then select [this file](./Public-Safety-Alerts.swagger.json), like [how this image shows](https://i.imgur.com/cNFkzYs.png).
- The `.msapp` file for the completed Canvas App can be downloaded [here](https://github.com/TimHanewich/Power-Platform-Assets/releases/download/14/Citizen.Safety.Command.Center.msapp).

If deploying the finished solution using the completed resources above, be sure to import the custom connector first, *and then* the canvas app. When you are opening the canvas app file, you will see that it is unable to find the custom connector it references. This is because it hasn't found the **exact** custom connector (it references an old custom connector GUID). Simply click "Don't Allow" to insist it does not try to connect to the custom connector. This will break the references, but then you can go ahead and set them back up against your new connector.

**Tip on deployment**: If you name the new custom connector you import to "Public Safety Alerts" or "PublicSafetyAlerts" (the same name as the one I named and developed against), you will not need to do any re-references as the pre-built app above is already set up to use a custom connector with this name!

For demo purposes, I am [also providing this stripped version of the canvas app](https://github.com/TimHanewich/Power-Platform-Assets/releases/download/15/Citizen.Safety.Command.Center.Incomplete.msapp). This version of the app has the structure and assets of the first app, but is completely stripped of all references to any connectors. This is a great place to start from in a demonstration. You'll see, in the [gallery](./gallery.md), that it also specifically notes with comments where those custom connector actions should take place.

## Endpoint Services, Documented
The [Azure Functions-based API](./src/api/) has several endpoints that are described below. Please note that the endpoints provided below are **live**. You are welcome to use them for testing purposes. Please do not abuse them.

### Get Public Safety Alerts
This endpoint returns a list of all Public Safety Alert records as a JSON array.

Example request:
```
GET https://publicsafetyalerts.azurewebsites.net/alerts/
```

Example response:
```
200 OK

[
    {
        "Id": "B32C83",
        "UtcTime": "2024-02-01T15:53:53.0304463Z",
        "IssuingAuthority": "Public Alert Coordination Unit (PACU)",
        "AlertType": "Power Outage Alert",
        "AffectedRegions": "Ivydale City"
    },
    {
        "Id": "F30F9B",
        "UtcTime": "2024-02-01T15:57:10.5581563Z",
        "IssuingAuthority": "Emergency Preparedness Authority (EPA)",
        "AlertType": "Evacuation Order",
        "AffectedRegions": "Sunflower County"
    },
    {
        "Id": "15834A",
        "UtcTime": "2024-02-01T15:57:14.076258Z",
        "IssuingAuthority": "Regional Emergency Response Agency (RERA)",
        "AlertType": "Civil Disturbance Advisory",
        "AffectedRegions": "Sunset Valley"
    }
]
```

**Tip: You can also extend the URL with `/clear` to delete all public safety records in the database (GET request to `https://publicsafetyalerts.azurewebsites.net/alerts/clear`).**

### Create a new (random) Public Safety Record
You can also send a `POST` request to the `/alerts` endpoint to create a new Public Safety Record. This service exists, both to demonstrate the use of **action parameters** in Power Automate **and** to demonstrate the triggering of workflows subscribed to the webhook service below. After subscribing your endpoint service (Power Automate workflow) to the webhook service below, you can make a request to this endpoint to 1) have a new Public Safety Record created, 2) have this new PSA record saved to the database, and 3) alert every subscribed endpoint service of this new Public Safety Alert record.

Example request:

```
POST https://publicsafetyalerts.azurewebsites.net/alerts

{
    "IssuingAuthority": "Federal Safety Commission (FSC)",
    "AlertType": "Bear on the loose",
    "AffectedRegions": "Everywhere"
}
```

The request above will create a new Public Safety Alert record with the supplied data above. The `Id` and `UtcTime` properties of the Public Safety Alert will be filled in appropriately.

Please keep in mind that you do **not have to** specify any of the three properties above. If one is ommitted, the Public Safety Alert record that was created will be filled in with random data. So, even if a `POST` request is made to that endpoint with an empty JSON body or even *no body at all*, a random record will still be made and the subscribed endpoints will still be notified of this new record.

Example response (this response body is purely for informational purposes):
```
200 OK

{
    "newRecord": {
        "Id": "885C70",
        "UtcTime": "2024-02-02T19:50:00.7421949Z",
        "IssuingAuthority": "Citizen Welfare and Security Office (CWSO)",
        "AlertType": "Evacuation Order",
        "AffectedRegions": "Crestwood Municipality"
    },
    "webhookSubscribersNotifiedCount": 2,
    "webhookSubscribersNotified": [
        {
            "Id": "C0C9119598",
            "Endpoint": "https://webhook.site/db35115b-5330-4635-80b4-1786776ee54b"
        },
        {
            "Id": "C400E1A4B0",
            "Endpoint": "https://powerautomate.com/trigger_me_here_example"
        }
    ]
}
```
You can see in the response above, it shows the new record that was made and saved to the DB, a count of how many subscribed endpoints were notified of this new Public Safety Alert record, and each "notified" subscribed endpoint listed (both the endpoint itself and the ID of that particular webhook subscription). 

### Webhook Service: Subscribe
This endpoint allows you to *subscribe* your own web service to get updates on the addition of new Public Safety Alert records. This is the endpoint that Power Automate will call to to subscribe a workflow to these updates.

Example request:

```
POST https://publicsafetyalerts.azurewebsites.net/subscribe

{
    "url": "https://MyCoolWorkflow.com/example_workflow_trigger"
}
```

The request above will subscribe the endpoint `https://MyCoolWorkflow.com/example_workflow_trigger` to future updates of new Public Safety Alert records. 

The response to the request above will look like the following:
```
201 CREATED
Location https://publicsafetyalerts.azurewebsites.net/unsubscribe/4BE194C820
```

The `Location` header above is crucial - this is the *exact* endpoint that the subscribed webhook service (Power Automate in the case of this demo) will send a `DELETE` HTTP request to, requesting it be unsubscribed from the webhook service. **Thus, it is vital that this service provides a `Location` endpoint that is *unique* to the newly subscribed service (from this request).**

Once subscribed, once new Public Safety Alerts are created via the `POST /alerts` endpoint above, this is what the HTTP request delivered to the subscribed service will look like:
```
POST https://MyCoolWorkflow.com/example_workflow_trigger

{
  "Id": "848903",
  "UtcTime": "2024-02-01T19:31:46.1824534Z",
  "IssuingAuthority": "Municipal Safety Council (MSC)",
  "AlertType": "Cybersecurity Breach Notification",
  "AffectedRegions": "Sunset Valley"
}
```
As seen above, it is only a single Public Safety Alert record, the new one that was just made.

And, if you're curious about how Power Automate will respond to this POST request notification, it will simply return a `202 ACCEPTED` response, indicating that it has accepted the new record and thus triggered the subscribed workflow:

![triggered workflow](https://i.imgur.com/zrkG8Vw.png)

### Webhook Service: Unsubscribe
Eventually, the subscribed webhook will no longer want updates! This could be caused from either the Power Automate dev turning off the subscribed flow or deleting it altogether. When either of these happen, Power Automate will automatically make a call to your service, asking for the endpoint (flow trigger) it previously subscribed, to be unsubscribed from future updates!

As described above, it will specifically use the **exact** URL that the webhook service provided to it as the `Location` header when it originally subscribed. It will send a single DELETE request to this URL.

In the example in the `/subscribe` endpoint description above, Power Automate would make an HTTP DELETE request to the endpoint as such:
```
DELETE https://publicsafetyalerts.azurewebsites.net/unsubscribe/4BE194C820
```
Again, the URL must be unique to *a specific* subscribed endpoint (flow trigger); thus, in this case, the `4BE194C820` appended at the end serves as a unique identifier, identifying the specific subscribed endpoint to unsubscribe. The response will be `200 OK`, confirming the webhook subscription of the supplied ID has been unsubscribed and will no longer receieve notifications.

The `/unsubscribe` endpoint is supposed to only be used to unsubscribe a specific, single endpoint. However, I also built in functionality for unsubscribing *all* subscribed endpoints in one call, purely for ease of demo purposes. Simply replace the unique webhook subscription ID with `all`:
```
DELETE https://publicsafetyalerts.azurewebsites.net/unsubscribe/all
```

The request above will again return `200 OK`, confirming all subscribed webhooks have been deleted.

So what happens if, for whatever reason, the unsubscribe process did not work? And the flow is deleted but your webhook service continues to try to make calls to it when new Public Safety Alert records are created? In this event, the Power Automate service will return a `400 BAD REQUEST` response, instead of the expected `202 ACCEPTED` if it was accepted (triggered a workflow) successfully:

![workflow has been deleted and thus has been unsubscribed](https://i.imgur.com/aGYMmaw.png)

This is Power Automate's way of reminding your webhook service that this flow has previously asked to be unsubscribed, so it no longer wants updates.

**In Power Automate, for the workflow that was previously subscribed to be successfully unsubscribed when the flow is deleted or turned off, this `/unsubscribe` endpoint needs to be registered as an "internal" action**. Example image below: 

![internal action](https://i.imgur.com/dwMEgG6.png)

Please also note the URL in the image above. For Power Automate to recognize a portion of the path as a parameter, you must pass it in within curly brackets!

### Version
Gets the version of the API you are using (in case you want to verify a update you deployed is now live, increment this before deploying).

Example request:
```
GET https://publicsafetyalerts.azurewebsites.net/version
```

Example response:
```
200 OK

0.1.0
```

## VS Code to Azure Functions Deployment Bug Found on February 2, 2024
Just leaving this here as a note in case it is encountered again:

I noticed a bug when deploying this [API](./src/api/) to Azure Functions in Azure via VS Code. When I created a new Windows-based Azure Function app in Azure and tried deploying, it would indicate the deployment was successful in the output window in VS Code, but no HTTP functions would actually be registered and visible under the Azure Function in the Azure portal. Depsite them working perfectly during testing locally.

I tried many different things. Finally, after creating a *new* Azure Function deployment in Azure, this time a **linux-based Azure Function** deployment, and then trying to deploy again to the function app via VS Code, it *did* work. Seems there may be a bug with deploying to Windows-based Azure Functions at the moment.

Further context on when this was discovered:
- Using Azure Functions Core Tools version 4.0.5455
- .NET 7.0 (Isolated)

## Appendix Resources
- Full PSA Graphic: https://i.imgur.com/QrTUJnQ.png
- Safety Shield Graphic: https://i.imgur.com/SlSyzgw.png
    - Against white background (use for custom connector icon): https://i.imgur.com/5UcTjop.png