# Environmental Air Quality Monitoring
![AQM](https://i.imgur.com/ifQcQSQ.png)
This is a solution that showcases how State & Local Government's can monitor environmental air quality with:
- A **Raspberry Pi Pico W**, serving as an IoT device, that collects air quality data via an **ENS160** and **DHT-22** sensor
- A **Power Automate** flow that receives this data from the Raspberry Pi via an HTTP POST request trigger
- A **Dataverse** table that the Power Automate flow places the data into, using several *Formula columns*
- A **Power Apps** *Model-Driven App* to display collected Air Quality Reading Data
- A **Power BI** dashboard that reports air quality data and trends in a visual report, reading directly from Dataverse.
- A **Power Apps** *Canvas App* with the embedded Power BI report and further "dashboard-like" indicators.
- A **Power Automate** flow that serves the latest air quality data programmatically via API call.

## Why is Environmental Air Quality Important to SLG?
- **Protect Public Health** - Poor air quality is a major environmental risk to human health, particularly for vulnerable populations such as children, older adults, and those with pre-existing medical conditions. Exposure to polluted air can lead to respiratory problems, cardiovascular disease, and even premature death. By tracking ambient air quality, you can identify areas of concern and take proactive measures to mitigate the health impacts of poor air quality.
- **Improve Environmental Sustainability** - Ambient air quality tracking is a critical component of environmental sustainability efforts. By monitoring and addressing air quality issues, you can reduce the environmental footprint of your community and promote a healthier, more sustainable environment for future generations.
- **Support Economic Develoment** - Clean air is essential for attracting businesses, tourists, and residents to your community. Areas with poor air quality can experience reduced economic growth, lower property values, and decreased quality of life. By demonstrating a commitment to air quality monitoring and improvement, you can enhance your community's reputation and competitiveness.
- **Meet Regulatory Requirements** - State and local governments are responsible for implementing federal air quality standards and regulations. Effective tracking and monitoring of ambient air quality conditions are critical to meeting these requirements and avoiding non-compliance penalties.

## The Possibilities in SLG
While this example showcases using IoT technology paired with Power Platform to track *air quality*, there are many use cases for the application of this technology through State & Local government. For example:
- **Infrastructure Management**: IoT devices monitor the condition of bridges, roads, and public buildings. They can detect structural issues, track usage patterns, and predict maintenance needs, ensuring public safety and optimizing infrastructure investments.
- **Smart Parking**: IoT sensors in publicly-owned parking garages/lots can detect occupancy and transmit data to centralized systems. This information helps drivers find available parking spots more efficiently, reducing traffic congestion and emissions.
- **Public Safety and Security**: IoT devices such as cameras, sensors, and smart streetlights enhance public safety. They can detect incidents, monitor crowds, analyze traffic patterns, and improve emergency response times.
- **Water Management**: IoT sensors monitor water usage, detect leaks, and manage irrigation systems in parks and public spaces. This promotes water conservation and reduces operational costs for municipalities.
- **Transportation** and Traffic Management: IoT technology can monitor traffic flow, manage traffic signals dynamically, and provide real-time updates to commuters. This improves transportation efficiency, reduces congestion, and enhances road safety.
- **Energy Efficiency**: IoT sensors in public buildings monitor energy consumption, optimize HVAC systems, and control lighting based on occupancy. This reduces energy costs, enhances sustainability efforts, and lowers carbon footprints.
- **Waste Management**: IoT-enabled bins monitor waste levels and optimize collection routes. This reduces operational costs, minimizes overflow incidents, and promotes efficient waste disposal practices.

## Solution Gallery
I've included some pictures below of this solution in action, broken into its functional components!

### The Raspberry Pi Pico W, DHT-22, and ENS160 sensor it its 3D-printed housing
After soldering the Raspberry Pi Pico W with the ENS160 and DHT-22 sensors, they are neatly placed in 3D-printed housing:

![housing](https://i.imgur.com/KcKyxU2.jpeg)

I then mount these on the outside wall of my home where it can observe external conditions:

![mounted](https://i.imgur.com/xvwSLxR.png)

### The IoT device sends sample data to Power Platform via HTTP POST request to a Power Automate workflow
The MicroPython script that runs on the Raspberry Pi will record the ambient air quality conditions once per minute and perform an HTTP POST request to an endpoint that will trigger a Power Automate flow. The body of the POST request will contain the sensor data in the following format:

```
{
    "temperature": 92.4,
    "humidity": 0.63,
    "aqi": 2,
    "tvoc": 843,
    "eco2": 481
}
```

The data will subsequently be parsed and inserted as a new row into Dataverse:

![Power Automate post](https://i.imgur.com/My3Qeka.png)
 
### A Dataverse "Formula" Column is used to calculate the **Absolute Humidity** 
A Dataverse **Formula Column** is used to calculator *Absolute Humidity* two known values: *Relative Humidity* and *Temperature*. This uses a rather complex formula that can be read about further towards the bottom of this document (see below).

![dataverse formula](https://i.imgur.com/e5NtGmY.png)

### Air Quality Reading Data is available via Model-Driven App
A model-driven Power App displays a simple yet effective interface for showing **Air Quality Reading** records received from the IoT device:

![md data](https://i.imgur.com/dRHDaOQ.png)

![md app form](https://i.imgur.com/Zja5WAf.png)

### A Power BI Report natively connects to the data in Dataverse 
Using Power BI, we can easily connect a new report to our data that lives in Dataverse and display it on a few charts:

![power BI report](https://i.imgur.com/Ol7l3ld.png)

### The Power BI Dashboard is embedded in a Power App as a "Dashboard"
We can easily embed that Power BI report into a Power App Canvas app, with additional peripheral functionality, to create a nifty dashboard.

![embedded in canvas app](https://i.imgur.com/ad8Eo2D.png)

### Serving Data via Power Automate API Endpoint
A Power Automate flow is configured with an HTTP Trigger and HTTP response, allowing us to create a makeshift API endpoint that can return the most recent Air Quality Reading data (click [here](https://prod2-21.usgovtexas.logic.azure.us:443/workflows/4839d796eecb4d5f9048d015f9d4878c/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=1NYbRqQP8uddqPoncZLRXdFeKuNCMW1c-1BmfGUYidw) to call to this endpoint!):

![air quality reading data endpoint](https://i.imgur.com/CplQ8Tv.png)

The data will be returned as JSON with the following format:

```
{
    "sampled": "2024-08-15T14:31:24Z",
    "temperaturef": 83.66,
    "relhumidity": 0.45,
    "abshumidity": 0.1273,
    "aqi": 2,
    "tvoc": 117,
    "eco2": 578
}
```

## Assets
We're providing the following assets so you can replicate this project as well:
- The Raspberry Pi IoT device lives neatly in a 3D-printed housing. You can find the source files (.STL) files of the housing and print yourself [on Thingiverse](https://www.thingiverse.com/thing:6655576).
- The source code (written in MicroPython) that runs on the Raspberry Pi Pico W can be found in the [`src` folder](./src/)
- The Power Platform solution (.zip file)
    - [Version 1.0.0.3](https://github.com/microsoft/SLG-Business-Applications/releases/download/9/AirQualityMonitoring_1_0_0_3.zip) - base solution (table, cloud flow, model-driven app)
    - [Version 1.0.0.4](https://github.com/microsoft/SLG-Business-Applications/releases/download/12/AirQualityMonitoring_1_0_0_4.zip) - Added "Air Quality Monitoring Dashboard" Canvas App and "Get most recent AQR" Power Automate flow (serves data via HTTP call).
- The Power BI Report can be downloaded [here](https://github.com/microsoft/SLG-Business-Applications/releases/download/13/air_quality_dashboard.pbix).

## How to Calculate Absolute Humidity from Relative Humidity and Temperature
In this project, we calculate *absolute* humidity from the *relative humidity* and *temperature* readings our IoT device samples and transmits. Below is the equation for calculating absolute humidity. We use this equation in our Dataverse **Formula** column.

![formulas](https://i.imgur.com/1aRPvRh.png)

Temperature is in Celsius, Relative Humidity as a percentage.

You can express the formula above as a Dataverse Formula column like so:
```
(6.112 * (2.71828^((17.67*(('Temperature, Fahrenheit'-32)*(5/9)))/((('Temperature, Fahrenheit'-32)*(5/9))+243.5))) * 'Relative Humidity' * 2.1674) / (273.15 + (('Temperature, Fahrenheit'-32)*(5/9)))
```

The above formulas are from [here](https://carnotcycle.wordpress.com/2012/08/04/how-to-convert-relative-humidity-to-absolute-humidity/).

## Power BI Expression for Converting to EST
To convert the `createdon` field (stored in UTC time) to EST:

```
[createdon] - #duration(0, 4, 0, 0)
```

## Other Misc Resources
- All photos of this project: https://imgur.com/a/9LA9yLG
- PowerPoint decks used to design graphics seen above and in other places:
    - [Wide banner](https://github.com/microsoft/SLG-Business-Applications/releases/download/10/eq.pptx)
    - [Taller version of the banner grahic above](https://github.com/microsoft/SLG-Business-Applications/releases/download/11/eq2.pptx)

## Credit
Solution created by [Tim Hanewich](https://github.com/TimHanewich).