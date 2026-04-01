# Your First Power Apps Code App
![Image 1](https://i.imgur.com/wLlIYr6.jpeg)

[Power Apps Code Apps](https://learn.microsoft.com/en-us/power-apps/developer/code-apps/overview) allows developers to bring custom React applications into the Power Platform, integrated with key Power Platform services like Dataverse, connectors, and Entra ID authentication.

While there is no definitive date for the arrival of code apps in our soveirgn clouds (GCC), it will be arriving soon.

This article takes you through the basics of creating your first code app, step-by-step!

## The Plan
![Image 2](https://i.imgur.com/jYrbGF5.jpeg)
We will follow an incremental 6-step plan to get you up and running with your first code app. While AI tools like GitHub Copilot and others have shown incredible capabilities in handling the entire end-to-end process (or at least a good chunk of it), we will walk through each, step by step so you *understand* what is going on in each step.

## Install Dependencies
![Image 3](https://i.imgur.com/CTKmYyM.jpeg) 
First, you must install several dependencies onto your machine:
- **VS Code**(or any IDE of your choice!)
    - https://code.visualstudio.com/download
- **NodeJS**
    - JavaScript runtime, required for developing React apps
    - https://nodejs.org/en
- **Power Platform PAC CLI**
    - Required to eventually “push” the Code App to your Environment
    - https://learn.microsoft.com/en-us/power-platform/developer/howto/install-cli-msi
- **GitHub Copilot**
    - Within VS Code (as extension) or GitHub Copilot CLI
    - You don’t necessarily need this to make a Code App… as long as you are a good React dev 
    - https://code.visualstudio.com/docs/copilot/overview


## Enable Code Apps in your Environment
![Image 4](https://i.imgur.com/oAGi2BV.jpeg)
Before we begin building apps, it is important to note that as of the time of this writing you have to *enable* code apps in your target Power Platform environment. For whatever environment you wanted to host your first Code App in, visit the PPAC and find the "Power Apps Code Apps" toggle in the environment's settings!

## Create a React App
We are now ready for the first step in app creation: make a React app! Run `npm create vite@latest Cat-Facts` in your terminal to scaffold a "boilerplate" React app.
![Image 5](https://i.imgur.com/Sia5lZQ.jpeg)

After you do so, you should see it instantly serve the app for you on localhost. It provides a URL for you to navigate to in a browser to see your app.
![Image 6](https://i.imgur.com/tMHg0YJ.jpeg)

## Use AI to Customize your React App
With a basic React app now scaffolded, we can now proceed to build a React app that actually does something for us! If you are a talented React developer, you can certainly get to work writing your code at this stage. But for folks like me that aren't familiar React code, you can use AI to assist with development. Below I'm running `copilot` in the terminal to open GitHub Copilot to assist with writing my app.

![Image 7](https://i.imgur.com/2Gsl3mj.jpeg)

Once GitHub Copilot loads, I am running a simple prompt like the following:
```
Make me a “Random Cat Facts” app. When a user clicks on a button, they are shown a random cat fact.
```

![Image 8](https://i.imgur.com/Lv53WnL.jpeg)

And allowing it to do its thing, it confirms it is complete!


![Image 9](https://i.imgur.com/DnJ6vsl.jpeg)

Next, run `npm run dev` in the terminal to test out your new React app!

![Image 10](https://i.imgur.com/HFidmnE.jpeg)

Vite will now serve the React app to you on a local server for you to navigate to in the browser to test.
![Image 11](https://i.imgur.com/OHvEyIF.jpeg)
![Image 12](https://i.imgur.com/bm73gEv.jpeg)

## Prepare your React App to be a Power Apps Code App
Congrats, you have made your first React app! But the React app is not ready to be a *Power Apps Code App* just yet. To integrate the React app with Power Platform services (and allow it to be hosted as a Power Apps Code App), there are a few remaining steps:

First, install two packages in your React app from **npm**:
1. Add the [@microsoft/power-apps](https://www.npmjs.com/package/@microsoft/power-apps) npm package to the project by running `npm install @microsoft/power-apps` in your terminal.
2. Add the [@microsoft/power-apps-vite](https://www.npmjs.com/package/@microsoft/power-apps-vite) npm package to the project by running `npm install @microsoft/power-apps-vite` in your terminal.

![Image 13](https://i.imgur.com/OvjckCR.jpeg)

After both packages install, add the following highlighted portions to your `vite.config.ts` file in your app (see screenshot). For reference, I've included the entire contents of my file *after* the changes below (you can just replace your entire `vite.config.ts` file with this).

```ts
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import { powerApps } from "@microsoft/power-apps-vite/plugin"

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), powerApps()],
});
```

![Image 14](https://i.imgur.com/Fr8uIGT.jpeg)

And then run `pac code init -n "Fun Cat Facts"` in your terminal to set up the app's `power.config.json` file with Power Apps-related information about the app.

![Image 15](https://i.imgur.com/oygJRO1.jpeg)

Great! Your React app is now a **Code App**! Run `npm run dev` again to test it out. This time, you will see there is also a Power Apps "Local Play" URL. You can navigate to this URL in your browser to test out your code app as a Power Apps Code App!

![Image 16](https://i.imgur.com/AJlv6gd.jpeg)
![Image 17](https://i.imgur.com/BLjSGvv.jpeg)

## Push to your Environment
After testing locally, run `npm run build` followed by `pac code push` to push your code app to your target Power Platform environment!

![Image 18](https://i.imgur.com/nQyLlft.jpeg)

The PAC CLI will confirm it pushed successfully and provide a URL that you can navigate to to play it.

![Image 19](https://i.imgur.com/TQBqmNj.jpeg)

## Connect with Dataverse
Ok - this Cat Facts app is fun, but how do we make it *real*? How can we, for example, connect our app to Dataverse and perform Create, Read, Update, Delete (CRUD) operations on it?

Run `pac code add-data-source –a dataverse –t contact` to connect your app with the **Contacts** Dataverse Table.
![Image 20](https://i.imgur.com/huDtJEI.jpeg)

You will see running this command will add schema and service files to your app that allow the app to interface with the data in the Contacts table.
![Image 21](https://i.imgur.com/e65EZQX.jpeg)

Microsoft provides terrific documentation on how code in your Code App can be written to interface with data in the Contacts table. See here if interested in reviewing yourself: [How to: Connect your code app to Dataverse](https://learn.microsoft.com/en-us/power-apps/developer/code-apps/how-to/connect-to-dataverse)

![Image 22](https://i.imgur.com/5yNDGkZ.jpeg)

Since I'll be relying on GitHub Copilot to write my app's code for me, I'll make sure to provide this documentation to the AI before instructing it to make my app for me. By explicitly providing this documentation, I am **ensuring** GitHub Copilot has the latest documentation and best-practices to follow.

Provide the following prompt to GitHub Copilot:

```
First, read this documentation on how to interface with
Dataverse: https://learn.microsoft.com/en-us/power-apps/developer/code-apps/how-to/connect-to-dataverse

I have already added the Contact table to this app.

Please make another page of this app in which it allows a user to Create, Read, Update, Delete records in the Contact table
```

![Image 23](https://i.imgur.com/u9CPQzk.jpeg)

Again, after allowing GitHub Copilot to do its thing for several minutes, it confirms it has completed the work!

![Image 24](https://i.imgur.com/76lrNop.jpeg)

Next step is to test out the app: run `npm run dev` and Vite will serve up your app locally. Navigate to the Power Apps "Local Play" URL in your browser.

![Image 25](https://i.imgur.com/dw8yCIj.jpeg)

Your app should be updated with an additional page that allows you to interface with data in your Contacts table.

![Image 26](https://i.imgur.com/X8zQekz.jpeg)

## Push to your Environment
And, just like last time, we are ready to push our app to our Power Platform environment!

Run `npm run build` followed by `pac code push` to push your Code App to your target Power Platform environment.

![Image 27](https://i.imgur.com/oOT1tDW.jpeg)

![Image 28](https://i.imgur.com/PeWUzsN.jpeg)

## Additional Resources
- Imgur album: https://imgur.com/a/vOAGF0U