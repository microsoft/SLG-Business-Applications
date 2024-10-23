# ![Copilot Studio Advanced Analytics](https://i.imgur.com/X8s6bNH.jpeg)

[![csaa_app](https://i.imgur.com/CrVSyzh.png)](https://youtu.be/Rj9aH3xAggo)

*Click the image above for a demo video!*

This demonstration application showcases how professional developers can authenticate into Dataverse, extract Copilot Studio telemetry logs and transcripts, and write code to analyze this data to uncover key insights.

This application provides the following out-of-the-box functionality:
- 📈 Review environment-level statistics, including total number of bots, sessions, messages, and daily message consumption.
- 🔍 View a breakdown of bot ownership, detailing which users own specific bots.
- 📜 Examine the message consumption and transcripts of any bot in your environment.
- 🤖 Review the source content used by generative AI and the corresponding answers produced from that content.
- ☁️ Archive Copilot Studio session transcripts to Azure Blob Storage.

## How to run this application
The application in the [src](./src/) is a .NET Console Application. It is based on the [.NET 8.0](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8/overview) runtime, so you will need the **.NET 8.0** SDK to run it. You can download and install the .NET 8.0 SDK from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

The app will need your credentials to your environment to be able to extract telemetry out of Dataverse. You can provide this in one of two ways:

1. Place all secrets in the `keys.json` file in the root directory of the console app.
2. Let the application ask you for these credentials each time it runs, one by one.

If you choose to go with #1 (recommended), place your keys in a file called `keys.json` in the root directory of the console app (i.e. right next to the "Program.cs" file, in that same directory).

The `keys.json` file should have this structure:
```
{
    "username": "<your username>",
    "password": "<your password>",
    "environment": "<your environment URL>",
    "azblob": "<your azure blob storage connection string>"
}
```

The program will open this file and parse the credentials, removing the need for you to enter in the credentials one by one every time you run the application. Again, alternatively, you can instead just opt to ignore this and enter them in each time yourself manually when it prompts you to.

**With the .NET 8.0 SDK installed on your PC (you can confirm this by running `dotnet --list-sdks`), use `cd` to navigate to the [src](./src/) folder, and then simply enter `dotnet run`!**

## Example Telemetry Content
Copilot Studio logs telemetry to Dataverse via the `conversationtranscripts` table. You can find an example read of **all** `conversationtranscript` records in the [conversationtranscripts.json](./examples/conversationtranscripts.json) file in the `/examples` folder.

Observing the `conversationtranscript` record, you'll see that the actual *telemetry* from the Copilot's conversation is stored as an semi-structured JSON-encoded string in the `conversationtranscript`'s `content` property!

This `content` property is what contains most of the valuable telemetry we can leverage to learn more about our copilot's interactions with users! You can see an examples of this `content` property, parsed as a JSON object, in the [content folder](./examples/content/) file in the `/examples` folder.

## Disclaimer
This application is provided as a demonstration tool and is intended for informational and educational purposes only. It downloads telemetry data from Dataverse and generates insights based on that data. Use of this application is entirely at your own risk. There are no guarantees or warranties, expressed or implied, regarding the functionality, accuracy, or reliability of the insights generated by this application.

Neither the developer nor Microsoft will be held responsible for any errors, data loss, or other issues that may arise from the use of this application. This includes, but is not limited to, incorrect insights, system errors, or any impact on data in your environment.

## Other Resources
- [This .zip folder](https://github.com/microsoft/SLG-Business-Applications/releases/download/15/BACKUP.DATA.FOR.CSAA.zip) contains raw dumps of `bots`, `conversationtranscripts`, and `systemusers`, the three Dataverse tables that are used in this application. If you do not have good data in your Dataverse environment to work with, you could use this and configure the data acquisition code to load this from local (instead of actually pulling from Dataverse). 
    - But if you do this, you may also need to adjust the date that is considered "today" (will have to write code to do this). This is because if you use this data far into the future as of the time of this writing, every bot + session will be old and the breakdown of "last 30 days, last 90 days, etc." will make ALL bots/sessions/messages land in the 180+ day category. Adjust it to October 22, 2024 (time of this writing) for a good breakdown.
- Here is the [.MP3 file](https://github.com/microsoft/SLG-Business-Applications/releases/download/15/Tech.Beats.mp3) of the background music used in the demo video above (it is AI generated).

## Credit
*This application was created by [Tim Hanewich](https://github.com/TimHanewich).*