# Copilot Studio Advanced Analytics

## Example Content
Copilot Studio logs telemetry to Dataverse via the `conversationtranscripts` table. You can find an example read of **all** `conversationtranscript` records in the [conversationtranscripts.json](./examples/conversationtranscripts.json) file in the `/examples` folder.

Observing the `conversationtranscript` record, you'll see that the actual *telemetry* from the Copilot's conversation is stored as an semi-structured JSON-encoded string in the `conversationtranscript`'s `content` property!

This `content` property is what contains most of the valuable telemetry we can leverage to learn more about our copilot's interactions with users! You can see an examples of this `content` property, parsed as a JSON object, in the [content folder](./examples/content/) file in the `/examples` folder.

## Analytics to collect
|Analytic|Sourced From|
|-|-|
|Conversation Start (DateTime)|"timestamp" of first "ConversationInfo"|
|Conversation End (DateTime)|"timestamp" of last "ConversationInfo"|
|Conversation Duration (timespan)|Conversation End - Conversation Start|
|Messages (array of `Message`)|Strip out messages|