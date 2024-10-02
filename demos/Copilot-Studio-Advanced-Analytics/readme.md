# Copilot Studio Advanced Analytics

## Example Content
Copilot Studio logs telemetry to Dataverse via the `conversationtranscripts` table. You can find an example read of **all** `conversationtranscript` records in the [conversationtranscripts.json](./examples/conversationtranscripts.json) file in the `/examples` folder.

Observing the `conversationtranscript` record, you'll see that the actual *telemetry* from the Copilot's conversation is stored as an semi-structured JSON-encoded string in the `conversationtranscript`'s `content` property!

This `content` property is what contains most of the valuable telemetry we can leverage to learn more about our copilot's interactions with users! You can see an example of this `content` property, parsed as a JSON object, in the [content.json](./examples/content.json) file in the `/examples` folder.